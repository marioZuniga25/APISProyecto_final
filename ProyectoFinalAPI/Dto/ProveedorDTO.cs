namespace ProyectoFinalAPI.Dto
{
    public class ProveedorDTO
    {
        public int idProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public List<string> nombresMateriasPrimas { get; set; }  // Nombres de las materias primas
        public List<decimal> preciosMateriasPrimas { get; set; } // Precios de las materias primas
        public List<string> unidadesMateriasPrimas { get; set; } // Unidades de las materias primas
    }
}
