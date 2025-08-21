using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasExamen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VentasExamen.Infrastructure.Data
{
    public class VentasDbContext : DbContext
    {
        public VentasDbContext(DbContextOptions<VentasDbContext> options) : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Empleado
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.IdEmpleado);
                entity.Property(e => e.IdEmpleado).HasColumnName("ID_EMPLEADO");
                entity.Property(e => e.Nombre).HasColumnName("NOMBRE").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Apellido).HasColumnName("APELLIDO").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Estado).HasColumnName("ESTADO").HasMaxLength(10).IsRequired()
                    .HasDefaultValue("activo");

                entity.Ignore(e => e.NombreCompleto);
            });

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.IdCliente);
                entity.Property(c => c.IdCliente).HasColumnName("ID_CLIENTE");
                entity.Property(c => c.Dni).HasColumnName("DNI").HasMaxLength(10).IsRequired();
                entity.Property(c => c.Nombre).HasColumnName("NOMBRE").HasMaxLength(50).IsRequired();
                entity.Property(c => c.Apellido).HasColumnName("APELLIDO").HasMaxLength(50).IsRequired();
                entity.Property(c => c.DireccionEnvio).HasColumnName("DIRECCION_ENVIO").HasMaxLength(100).IsRequired();

                entity.HasIndex(c => c.Dni).IsUnique();
                entity.Ignore(c => c.NombreCompleto);
            });

            // Configuración de Sucursal
            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.HasKey(s => s.IdSucursal);
                entity.Property(s => s.IdSucursal).HasColumnName("ID_SUCURSAL");
                entity.Property(s => s.Nombre).HasColumnName("NOMBRE").HasMaxLength(100).IsRequired();
                entity.Property(s => s.Direccion).HasColumnName("DIRECCION").HasMaxLength(100).IsRequired();
            });

            // Configuración de Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(p => p.IdProducto);
                entity.Property(p => p.IdProducto).HasColumnName("ID_PRODUCTO");
                entity.Property(p => p.Nombre).HasColumnName("NOMBRE").HasMaxLength(50).IsRequired();
                entity.Property(p => p.PrecioUnitario).HasColumnName("PRECIO_UNITARIO")
                    .HasColumnType("decimal(10,2)").IsRequired();
            });

            // Configuración de Venta
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(v => v.IdVenta);
                entity.Property(v => v.IdVenta).HasColumnName("ID_VENTA");
                entity.Property(v => v.FechaVenta).HasColumnName("FECHA_VENTA").IsRequired();
                entity.Property(v => v.IdEmpleado).HasColumnName("ID_EMPLEADO").IsRequired();
                entity.Property(v => v.IdCliente).HasColumnName("ID_CLIENTE").IsRequired();
                entity.Property(v => v.IdSucursal).HasColumnName("ID_SUCURSAL").IsRequired();
                entity.Property(v => v.ImporteTotal).HasColumnName("IMPORTE_TOTAL")
                    .HasColumnType("decimal(10,2)").IsRequired();

                // Relaciones
                entity.HasOne(v => v.Empleado)
                    .WithMany(e => e.Ventas)
                    .HasForeignKey(v => v.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.Cliente)
                    .WithMany(c => c.Ventas)
                    .HasForeignKey(v => v.IdCliente)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.Sucursal)
                    .WithMany(s => s.Ventas)
                    .HasForeignKey(v => v.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de DetalleVenta
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(dv => dv.IdDetalle);
                entity.Property(dv => dv.IdDetalle).HasColumnName("ID_DETALLE");
                entity.Property(dv => dv.IdVenta).HasColumnName("ID_VENTA").IsRequired();
                entity.Property(dv => dv.IdProducto).HasColumnName("ID_PRODUCTO").IsRequired();
                entity.Property(dv => dv.Cantidad).HasColumnName("CANTIDAD").IsRequired();
                entity.Property(dv => dv.PrecioUnitario).HasColumnName("PRECIO_UNITARIO")
                    .HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(dv => dv.Subtotal).HasColumnName("SUBTOTAL")
                    .HasColumnType("decimal(10,2)").IsRequired();

                // Relaciones
                entity.HasOne(dv => dv.Venta)
                    .WithMany(v => v.DetalleVentas)
                    .HasForeignKey(dv => dv.IdVenta)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(dv => dv.Producto)
                    .WithMany(p => p.DetalleVentas)
                    .HasForeignKey(dv => dv.IdProducto)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
