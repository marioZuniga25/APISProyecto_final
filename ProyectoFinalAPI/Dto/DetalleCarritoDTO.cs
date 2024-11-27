namespace ProyectoFinalAPI.Dto
{
    public class DetalleCarritoDTO
    {
        public int IdDetalleCarrito { get; set; }
        public int idProducto { get; set; }
        public string nombreProducto { get; set; } 
        public int cantidad { get; set; }
        public double precioUnitario { get; set; }
        public int EnPromocion { get; set; }
        public string imagen {get; set;}
         public string descripcion { get; set;}
        public DateTime fechaAgregado { get; set; }
    }
}
