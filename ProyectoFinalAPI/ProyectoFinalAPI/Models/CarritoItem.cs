namespace ProyectoFinalAPI.Models
{
    public class CarritoItem
    {
        public int id { get; set; }
        public int productoId { get; set; }
        public string nombreProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public string imagen { get; set; }
    }
}
