namespace ProyectoFinalAPI.Dto
{
 public class PromocionDto
 {
  public string Nombre { get; set; }
  public DateTime FechaInicio { get; set; }
  public DateTime FechaFin { get; set; }
  public double Descuento { get; set; }
  public List<int> ProductosIds { get; set; }
 }
}
