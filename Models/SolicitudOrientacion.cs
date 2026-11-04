
namespace MigrantID.Models
{
    public class SolicitudOrientacion
    {
        public int IdSolicitud { get; set; }

        public int IdUsuario { get; set; }

        public string Tema { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Pendiente";
    }
}
