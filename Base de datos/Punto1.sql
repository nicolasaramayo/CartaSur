-- Punto 1: Crear la tabla VENTAS e insertar registros de ejemplo

-- Crear la tabla VENTAS
CREATE TABLE VENTAS (
    ID_VENTA int NOT NULL PRIMARY KEY,
    Fecha_venta datetime,
    Dni_cliente varchar(10),
    Nombre_empleado varchar(100),
    Nombre_cliente varchar(100),
    Importe_total decimal(10,2),
    Direccion_envio_cliente varchar(100),
    Direccion_sucursal_venta varchar(100),
    Nombre_sucursal_venta varchar(100),
    Producto varchar(20),
    Cantidad int
);

-- Insertar registros de ejemplo
INSERT INTO VENTAS (ID_VENTA, Fecha_venta, Dni_cliente, Nombre_empleado, Nombre_cliente, 
                   Importe_total, Direccion_envio_cliente, Direccion_sucursal_venta, 
                   Nombre_sucursal_venta, Producto, Cantidad) 
VALUES 

(1, '2024-10-22 10:00:00', '12345678', 'Juan Perez', 'Maria Gonzalez', 
 1500.50, 'av corrientes 1234, CABA', 'San Martin 500, CABA', 'Sucursal Centro', 'Laptop', 1),


--  fecha con más ventas
(2, '2024-10-23 11:00:00', '99887766', 'Ana Lucrecia', 'Sofia Morales', 
 1200.00, 'av calchaqui 2000, Quilmes', 'Rivadavia 200, La Plata', 'Sucursal La Plata', 'tv', 2);