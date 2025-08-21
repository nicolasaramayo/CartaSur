-- Punto 2: Obtener la fecha en la que hubo más ventas

-- Opción 1: Obtener solo la fecha con más ventas
SELECT TOP 1 
    CAST(Fecha_venta AS DATE) AS Fecha,
    SUM(Cantidad) AS Cantidad_Ventas
FROM VENTAS 
GROUP BY CAST(Fecha_venta AS DATE)
ORDER BY SUM(Cantidad) DESC;

