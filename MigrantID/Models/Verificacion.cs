
namespace MigrantID.Models
{
    public class Verificacion
    {
        public int IdVerificacion { get; set; }

        public int IdDocumento { get; set; }

        public DateTime FechaVerificacion { get; set; } = DateTime.Now;

        public string Resultado { get; set; } = "Pendiente";

        public string Observacion { get; set; } = string.Empty;
    }
}
