namespace ProyectoFinalAPI.Dto
{
 public class DetallePromocionDto
 {
  public int IdDetallePromocion { get; set; }
  public int IdPromocion { get; set; }
  public int IdProducto { get; set; }
  public string NombreProducto { get; set; }
  public double PorcentajeDescuento { get; set; }
  public double PrecioFinal { get; set; }

  public DateTime FechaFinPromocion { get; set; }
 }

}
