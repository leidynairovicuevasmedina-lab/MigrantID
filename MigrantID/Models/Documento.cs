
namespace MigrantID.Models
{
    public class Documento
    {
        public int IdDocumento { get; set; }

        public string TipoDocumento { get; set; } = string.Empty;

        public string NumeroDocumento { get; set; } = string.Empty;

        public string PaisEmision { get; set; } = string.Empty;

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Estado { get; set; } = "En revisión";
    }
}
