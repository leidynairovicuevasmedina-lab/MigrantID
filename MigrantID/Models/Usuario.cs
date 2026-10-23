
namespace MigrantID.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Rol { get; set; } = "Usuario";

        public bool Activo { get; set; } = true;
    }
}
