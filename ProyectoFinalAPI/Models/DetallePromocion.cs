namespace ProyectoFinalAPI.Models
{
 public class DetallePromocion
 {
  public int IdDetallePromocion { get; set; }
  public int IdPromocion { get; set; } // Clave foránea hacia Promocion
  public Promocion Promocion { get; set; }
  public int IdProducto { get; set; } // Clave foránea hacia Producto
  public Producto Producto { get; set; }
  public double PorcentajeDescuento { get; set; } // Porcentaje de descuento aplicado
  public double PrecioFinal { get; set; }
 }
}
