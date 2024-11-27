namespace ProyectoFinalAPI.Models
{
    public class Carrito { 
        public int IdCarrito { get; set; } 
        public int IdUsuario { get; set; } 
           public DateTime FechaCreacion { get; set; } // Agregada
        public double Total { get; set; } 
    }
}
