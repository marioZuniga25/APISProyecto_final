namespace ProyectoFinalAPI.DTOs
{
    public class MateriaPrimaDto
    {
        public int idMateriaPrima { get; set; }
        public string nombreMateriaPrima { get; set; }
        public string descripcion { get; set; }
        public int idInventario { get; set; }
        public double cantidad { get; set; }  // Propiedad de cantidad
    }
}
