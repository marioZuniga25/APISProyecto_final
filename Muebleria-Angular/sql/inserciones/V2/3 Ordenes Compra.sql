INSERT INTO OrdenCompra (idProveedor, fechaCompra, usuario)
VALUES
(1, '2024-11-20 10:30:00', 'Angel'),
(2, '2024-11-21 14:15:00', 'Angel'),
(3, '2024-11-22 09:45:00', 'Angel'),
(4, '2024-11-23 11:20:00', 'Angel'),
(5, '2024-11-24 16:00:00', 'Angel'),
(6, '2024-11-25 13:10:00', 'Angel'),
(7, '2024-11-26 08:00:00', 'Angel'),
(8, '2024-11-27 15:45:00', 'Angel');


INSERT INTO DetalleOrdenCompra (idMateriaPrima, cantidad, precioUnitario, OrdenCompraidOrdenCompra)
VALUES
(1, 50, 50.00, 1), -- Madera de Pino de "Maderas y Tableros de México"
(2, 30, 120.00, 2), -- Madera de Roble de "Proveedora de Maderas del Norte"
(3, 100, 30.00, 3), -- MDF de "Fábrica de MDF Toluca"
(4, 500, 0.05, 4), -- Clavos de "Clavos y Tornillos Monterrey"
(5, 10, 15.00, 5), -- Pegamento de Madera de "Pegamentos y Barnices del Bajío"
(6, 5, 25.00, 6), -- Barniz Transparente de "Pegamentos y Barnices del Bajío"
(7, 300, 0.50, 7), -- Lijas de "Herramientas Especializadas Jalisco"
(8, 50, 2.50, 8), -- Bisagras de "Bisagras y Accesorios Querétaro"
(1, 20, 50.00, 1), -- Pedido adicional de Madera de Pino
(6, 2, 35.00, 6); -- Pintura para Madera de "Distribuidora de Pinturas DF"