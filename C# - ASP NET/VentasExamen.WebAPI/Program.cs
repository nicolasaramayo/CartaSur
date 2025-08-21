using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VentasExamen.Application.Interfaces;
using VentasExamen.Application.Services;
using VentasExamen.Application.Interfaces.Repositories;
using VentasExamen.Infrastructure.Data;
using VentasExamen.Infrastructure.Repositories;
using VentasExamen.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressConsumesConstraintForFormFileParameters = true;
        options.SuppressInferBindingSourcesForParameters = true;
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "VentasExamen API",
        Version = "v1",
        Description = "API ventas - Examen Técnico"
    });

    // Habilitar comentarios XML para documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Database config
builder.Services.AddDbContext<VentasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection - Repositories
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ISucursalRepository, SucursalRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// Dependency Injection - UOF
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Dependency Injection - Services
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

// CORS configuration for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Logging configuration
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});


//// Health checks
builder.Services.AddHealthChecks()
   .AddDbContextCheck<VentasDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VentasExamen API V1");
        c.RoutePrefix = string.Empty; // Para que Swagger esté en la raíz
    });
}

// Middleware pipeline
app.UseHttpsRedirection();

// CORS debe ir antes de Authorization
app.UseCors("AngularApp");

app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapHealthChecks("/health");

// Endpoint de prueba
app.MapGet("/", () => new
{
    message = "VentasExamen API funcionando",
    version = "1.0.0",
    timestamp = DateTime.UtcNow,
    swagger = "/swagger"
});

// Database initialization (opcional, para desarrollo)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<VentasDbContext>();
        try
        {
            // Aplicar migraciones automáticamente
            context.Database.EnsureCreated();

            // Seed data si no hay datos
            if (!context.Empleados.Any())
            {
                await SeedData(context);
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error al inicializar la base de datos");
        }
    }
}

app.Run();

// Método para insertar datos iniciales
static async Task SeedData(VentasDbContext context)
{
    // Datos iniciales basados en nuestros scripts SQL
    var empleados = new[]
    {
        new VentasExamen.Domain.Entities.Empleado { Nombre = "Juan", Apellido = "Pérez", Estado = "activo" },
        new VentasExamen.Domain.Entities.Empleado { Nombre = "Ana", Apellido = "López", Estado = "activo" },
        new VentasExamen.Domain.Entities.Empleado { Nombre = "Pedro", Apellido = "Martínez", Estado = "activo" },
        new VentasExamen.Domain.Entities.Empleado { Nombre = "María", Apellido = "García", Estado = "inactivo" }
    };

    context.Empleados.AddRange(empleados);
    await context.SaveChangesAsync();

    var clientes = new[]
    {
        new VentasExamen.Domain.Entities.Cliente
        {
            Dni = "12345678", Nombre = "María", Apellido = "González",
            DireccionEnvio = "Av. Corrientes 1234, CABA"
        },
        new VentasExamen.Domain.Entities.Cliente
        {
            Dni = "87654321", Nombre = "Carlos", Apellido = "Rodríguez",
            DireccionEnvio = "Belgrano 789, Quilmes"
        },
        new VentasExamen.Domain.Entities.Cliente
        {
            Dni = "11223344", Nombre = "Lucía", Apellido = "Fernández",
            DireccionEnvio = "Mitre 456, La Plata"
        },
        new VentasExamen.Domain.Entities.Cliente
        {
            Dni = "55667788", Nombre = "Roberto", Apellido = "Silva",
            DireccionEnvio = "San Martín 321, Avellaneda"
        },
        new VentasExamen.Domain.Entities.Cliente
        {
            Dni = "99887766", Nombre = "Sofía", Apellido = "Morales",
            DireccionEnvio = "Alsina 654, Quilmes"
        }
    };

    context.Clientes.AddRange(clientes);
    await context.SaveChangesAsync();

    var sucursales = new[]
    {
        new VentasExamen.Domain.Entities.Sucursal { Nombre = "Sucursal Centro", Direccion = "San Martín 500, CABA" },
        new VentasExamen.Domain.Entities.Sucursal { Nombre = "Sucursal La Plata", Direccion = "Rivadavia 200, La Plata" },
        new VentasExamen.Domain.Entities.Sucursal { Nombre = "Sucursal Quilmes", Direccion = "Alsina 100, Quilmes" }
    };

    context.Sucursales.AddRange(sucursales);
    await context.SaveChangesAsync();

    var productos = new[]
    {
        new VentasExamen.Domain.Entities.Producto { Nombre = "Laptop", PrecioUnitario = 1500.50m },
        new VentasExamen.Domain.Entities.Producto { Nombre = "Mouse", PrecioUnitario = 283.58m },
        new VentasExamen.Domain.Entities.Producto { Nombre = "Monitor", PrecioUnitario = 1150.00m },
        new VentasExamen.Domain.Entities.Producto { Nombre = "Teclado", PrecioUnitario = 150.05m },
        new VentasExamen.Domain.Entities.Producto { Nombre = "Webcam", PrecioUnitario = 600.00m }
    };

    context.Productos.AddRange(productos);
    await context.SaveChangesAsync();

    // Ventas con fechas que permiten probar el punto 2
    var ventas = new[]
    {
        new VentasExamen.Domain.Entities.Venta
        {
            FechaVenta = new DateTime(2024, 10, 23, 10, 30, 0),
            IdEmpleado = 1, IdCliente = 1, IdSucursal = 1, ImporteTotal = 1500.50m
        },
        new VentasExamen.Domain.Entities.Venta
        {
            FechaVenta = new DateTime(2024, 10, 23, 14, 15, 0),
            IdEmpleado = 2, IdCliente = 2, IdSucursal = 1, ImporteTotal = 850.74m
        },
        new VentasExamen.Domain.Entities.Venta
        {
            FechaVenta = new DateTime(2024, 10, 22, 16, 45, 0),
            IdEmpleado = 3, IdCliente = 3, IdSucursal = 2, ImporteTotal = 2300.00m
        },
        new VentasExamen.Domain.Entities.Venta
        {
            FechaVenta = new DateTime(2024, 10, 24, 9, 20, 0),
            IdEmpleado = 1, IdCliente = 4, IdSucursal = 1, ImporteTotal = 750.25m
        },
        new VentasExamen.Domain.Entities.Venta
        {
            FechaVenta = new DateTime(2024, 10, 23, 11, 0, 0),
            IdEmpleado = 2, IdCliente = 5, IdSucursal = 2, ImporteTotal = 1200.00m
        }
    };

    context.Ventas.AddRange(ventas);
    await context.SaveChangesAsync();

    // Detalle de ventas
    var detalleVentas = new[]
    {
        new VentasExamen.Domain.Entities.DetalleVenta
        {
            IdVenta = 1, IdProducto = 1, Cantidad = 1, PrecioUnitario = 1500.50m, Subtotal = 1500.50m
        },
        new VentasExamen.Domain.Entities.DetalleVenta
        {
            IdVenta = 2, IdProducto = 2, Cantidad = 3, PrecioUnitario = 283.58m, Subtotal = 850.74m
        },
        new VentasExamen.Domain.Entities.DetalleVenta
        {
            IdVenta = 3, IdProducto = 3, Cantidad = 2, PrecioUnitario = 1150.00m, Subtotal = 2300.00m
        },
        new VentasExamen.Domain.Entities.DetalleVenta
        {
            IdVenta = 4, IdProducto = 4, Cantidad = 5, PrecioUnitario = 150.05m, Subtotal = 750.25m
        },
        new VentasExamen.Domain.Entities.DetalleVenta
        {
            IdVenta = 5, IdProducto = 5, Cantidad = 2, PrecioUnitario = 600.00m, Subtotal = 1200.00m
        }
    };

    context.DetalleVentas.AddRange(detalleVentas);
    await context.SaveChangesAsync();
}