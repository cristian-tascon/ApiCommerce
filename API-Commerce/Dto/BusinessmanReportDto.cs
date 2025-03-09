namespace API_Commerce.Dto
{
    public class BusinessmanReportDto
    {
        public string Nombre_Comerciante { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo_Electronico { get; set; }
        public DateOnly Fecha_Registro { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int Cantidad_Establecimientos { get; set; }
        public decimal Total_Ingresos { get; set; }
        public int Cantidad_Empleados { get; set; }
    }
}
