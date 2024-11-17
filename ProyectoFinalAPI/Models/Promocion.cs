// Models/Promocion.cs
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalAPI.Models
{
    public class Promocion
    {
        [Key]
        public int IdPromocion { get; set; }
        public string Nombre { get; set; } // Nombre de la promoción
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public ICollection<DetallePromocion> Detalles { get; set; }
    }
}