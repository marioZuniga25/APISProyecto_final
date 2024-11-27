namespace ProyectoFinalAPI.Models
{
    public class DetalleCarrito
    {
        public int IdDetalleCarrito { get; set; }
        public int IdCarrito { get; set; } 
        public int IdProducto { get; set; } 
        public int Cantidad { get; set; } 
        public double PrecioUnitario { get; set; }
        public DateTime? FechaAgregado { get; set; } // Cambiado a nullable

    }
}
