namespace ProyectoFinalAPI.Dto
{
    public class CarritoDTO
    {
       public int IdCarrito { get; set; }
        public double total { get; set; }
        public int IdUsuario { get; set; } 
        public DateTime FechaCreacion { get; set; }
        public List<DetalleCarritoDTO> Detalles { get; set; }
    }
}
