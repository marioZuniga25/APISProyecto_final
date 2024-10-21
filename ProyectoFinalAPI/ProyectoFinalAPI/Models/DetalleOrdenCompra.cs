using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalAPI.Models
{
 public class DetalleOrdenCompra
 {
  [Key]
  public int idDetalleOrdenCompra { get; set; }
  public int idOrdenCompra { get; set; }
  public int idMateriaPrima { get; set; }
  public int cantidad { get; set; }
  public decimal precioUnitario { get; set; }

  [ForeignKey("idOrdenCompra")]
  public virtual OrdenCompra OrdenCompra { get; set; }

  [ForeignKey("idMateriaPrima")]
  public virtual MateriaPrima MateriaPrima { get; set; }
 }
}
