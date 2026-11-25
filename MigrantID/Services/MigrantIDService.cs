
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Usuario> Usuarios
        { get; } = new();

        public ObservableCollection<Verificacion> Verificaciones
        { get; } = new();

        public ObservableCollection<SolicitudOrientacion> Solicitudes
        { get; } = new();


  
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
                x => x.IdDocumento == id);
        }

        public bool ActualizarDocumento(Documento actualizado)
        {
            var existente =
                ObtenerDocumentoPorId(actualizado.IdDocumento);

            if (existente is null)
                return false;

            existente.TipoDocumento = actualizado.TipoDocumento;
            existente.NumeroDocumento = actualizado.NumeroDocumento;
            existente.PaisEmision = actualizado.PaisEmision;
            existente.FechaEmision = actualizado.FechaEmision;
            existente.FechaVencimiento = actualizado.FechaVencimiento;
            existente.Estado = actualizado.Estado;

            return true;
        }

        public bool EliminarDocumento(int id)
        {
            var documento = ObtenerDocumentoPorId(id);

            return documento is not null &&
                   Documentos.Remove(documento);
        }


        
        public Usuario AgregarUsuario(Usuario usuario)
        {
            usuario.IdUsuario = _siguienteIdUsuario++;
            Usuarios.Add(usuario);
            return usuario;
        }

        public Usuario? ObtenerUsuarioPorId(int id)
        {
            return Usuarios.FirstOrDefault(
                x => x.IdUsuario == id);
        }

        public bool ActualizarUsuario(Usuario actualizado)
        {
            for (int i = 0; i < Usuarios.Count; i++)
            {
                if (Usuarios[i].IdUsuario != actualizado.IdUsuario)
                    continue;

                Usuarios[i] = actualizado;
                return true;
            }

            return false;
        }

        public bool EliminarUsuario(int id)
        {
            var usuario = ObtenerUsuarioPorId(id);

            return usuario is not null &&
                   Usuarios.Remove(usuario);
        }


        
        public Verificacion AgregarVerificacion(
            Verificacion verificacion)
        {
            verificacion.IdVerificacion =
                _siguienteIdVerificacion++;

            Verificaciones.Add(verificacion);

            return verificacion;
        }

        public Verificacion? ObtenerVerificacionPorId(int id)
        {
            return Verificaciones.FirstOrDefault(
                x => x.IdVerificacion == id);
        }

        public bool ActualizarVerificacion(
            Verificacion actualizada)
        {
            for (int i = 0; i < Verificaciones.Count; i++)
            {
                if (Verificaciones[i].IdVerificacion !=
                    actualizada.IdVerificacion)
                    continue;

                Verificaciones[i] = actualizada;

                return true;
            }

            return false;
        }

        public bool EliminarVerificacion(int id)
        {
            var verificacion = ObtenerVerificacionPorId(id);

            return verificacion is not null &&
                   Verificaciones.Remove(verificacion);
        }


       
        public SolicitudOrientacion AgregarSolicitud(
            SolicitudOrientacion solicitud)
        {
            solicitud.IdSolicitud = _siguienteIdSolicitud++;

            Solicitudes.Add(solicitud);

            return solicitud;
        }

        public SolicitudOrientacion? ObtenerSolicitudPorId(int id)
        {
            return Solicitudes.FirstOrDefault(
                x => x.IdSolicitud == id);
        }

        public bool ActualizarSolicitud(
            SolicitudOrientacion actualizada)
        {
            for (int i = 0; i < Solicitudes.Count; i++)
            {
                if (Solicitudes[i].IdSolicitud !=
                    actualizada.IdSolicitud)
                    continue;

                Solicitudes[i] = actualizada;

                return true;
            }

            return false;
        }

        public bool EliminarSolicitud(int id)
        {
            var solicitud = ObtenerSolicitudPorId(id);

            return solicitud is not null &&
                   Solicitudes.Remove(solicitud);
        }
    }
}
