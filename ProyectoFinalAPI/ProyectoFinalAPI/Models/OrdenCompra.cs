using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalAPI.Models
{
 public class OrdenCompra
 {
  public int idOrdenCompra { get; set; }
  public int idProveedor { get; set; }
  [ForeignKey("idProveedor")]
  public Proveedor Proveedor { get; set; }
  public DateTime fechaCompra { get; set; }

  // Lista de detalles de la orden
  public ICollection<DetalleOrdenCompra> Detalles { get; set; }
 }

}
