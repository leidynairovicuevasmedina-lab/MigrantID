
using System.Collections.Generic;
using System.Linq;
using MigrantID.Models;

namespace MigrantID.Services
{
    public class MigrantIDService
    {

        public static MigrantIDService Instancia { get; } = new();

        public Documento? DocumentoEnEdicion { get; set; }

        private int _siguienteIdDocumento = 1;
        private int _siguienteIdUsuario = 1;
        private int _siguienteIdVerificacion = 1;
        private int _siguienteIdSolicitud = 1;

        public List<Documento> Documentos { get; } = new();

        public List<Usuario> Usuarios { get; } = new();

        public List<Verificacion> Verificaciones { get; } = new();

        public List<SolicitudOrientacion> Solicitudes { get; } = new();

        public Documento AgregarDocumento(Documento documento)
        {
            documento.IdDocumento = _siguienteIdDocumento++;

            Documentos.Add(documento);

            return documento;
        }

        public List<Documento> ObtenerDocumentos()
        {
            return Documentos.ToList();
        }

        public Documento? ObtenerDocumentoPorId(int id)
        {
            return Documentos.FirstOrDefault(
                documento => documento.IdDocumento == id);
        }

        public bool ActualizarDocumento(Documento documentoActualizado)
        {
            Documento? documentoExistente =
                ObtenerDocumentoPorId(documentoActualizado.IdDocumento);

            if (documentoExistente == null)
            {
                return false;
            }

            documentoExistente.TipoDocumento =
                documentoActualizado.TipoDocumento;

            documentoExistente.NumeroDocumento =
                documentoActualizado.NumeroDocumento;

            documentoExistente.PaisEmision =
                documentoActualizado.PaisEmision;

            documentoExistente.FechaEmision =
                documentoActualizado.FechaEmision;

            documentoExistente.FechaVencimiento =
                documentoActualizado.FechaVencimiento;

            documentoExistente.Estado =
                documentoActualizado.Estado;

            return true;
        }

        public bool EliminarDocumento(int id)
        {
            Documento? documento = ObtenerDocumentoPorId(id);

            if (documento == null)
            {
                return false;
            }

            return Documentos.Remove(documento);
        }

        public Usuario AgregarUsuario(Usuario usuario)
        {
            usuario.IdUsuario = _siguienteIdUsuario++;

            Usuarios.Add(usuario);

            return usuario;
        }

        public Verificacion AgregarVerificacion(Verificacion verificacion)
        {
            verificacion.IdVerificacion = _siguienteIdVerificacion++;

            Verificaciones.Add(verificacion);

            return verificacion;
        }

        public SolicitudOrientacion AgregarSolicitud(
            SolicitudOrientacion solicitud)
        {
            solicitud.IdSolicitud = _siguienteIdSolicitud++;

            Solicitudes.Add(solicitud);

            return solicitud;
        }
    }
}
