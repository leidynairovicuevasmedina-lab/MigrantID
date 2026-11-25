
using System.Collections.ObjectModel;
using System.Linq;
using MigrantID.Models;

namespace MigrantID.Services;

public class MigrantIDService
{
    public static MigrantIDService Instancia { get; } = new();

    private int _siguienteIdDocumento = 1;
    private int _siguienteIdUsuario = 1;
    private int _siguienteIdVerificacion = 1;
    private int _siguienteIdSolicitud = 1;

    public Documento? DocumentoEnEdicion { get; set; }

    public ObservableCollection<Documento> Documentos { get; } = new();
    public ObservableCollection<Usuario> Usuarios { get; } = new();
    public ObservableCollection<Verificacion> Verificaciones { get; } = new();
    public ObservableCollection<SolicitudOrientacion> Solicitudes { get; } = new();

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
        return Documentos.FirstOrDefault(x => x.IdDocumento == id);
    }

    public bool ActualizarDocumento(Documento documento)
    {
        var existente = ObtenerDocumentoPorId(documento.IdDocumento);

        if (existente == null)
            return false;

        var indice = Documentos.IndexOf(existente);
        Documentos[indice] = documento;

        return true;
    }

    public bool EliminarDocumento(int id)
    {
        var documento = ObtenerDocumentoPorId(id);

        if (documento == null)
            return false;

        return Documentos.Remove(documento);
    }

    public Usuario AgregarUsuario(Usuario usuario)
    {
        usuario.IdUsuario = _siguienteIdUsuario++;
        Usuarios.Add(usuario);
        return usuario;
    }

    public Usuario? ObtenerUsuarioPorId(int id)
    {
        return Usuarios.FirstOrDefault(x => x.IdUsuario == id);
    }

    public bool ActualizarUsuario(Usuario usuario)
    {
        var existente = ObtenerUsuarioPorId(usuario.IdUsuario);

        if (existente == null)
            return false;

        var indice = Usuarios.IndexOf(existente);
        Usuarios[indice] = usuario;

        return true;
    }

    public bool EliminarUsuario(int id)
    {
        var usuario = ObtenerUsuarioPorId(id);

        if (usuario == null)
            return false;

        return Usuarios.Remove(usuario);
    }

    public Verificacion AgregarVerificacion(Verificacion verificacion)
    {
        verificacion.IdVerificacion = _siguienteIdVerificacion++;
        Verificaciones.Add(verificacion);
        return verificacion;
    }

    public Verificacion? ObtenerVerificacionPorId(int id)
    {
        return Verificaciones.FirstOrDefault(x => x.IdVerificacion == id);
    }

    public bool ActualizarVerificacion(Verificacion verificacion)
    {
        var existente = ObtenerVerificacionPorId(verificacion.IdVerificacion);

        if (existente == null)
            return false;

        var indice = Verificaciones.IndexOf(existente);
        Verificaciones[indice] = verificacion;

        return true;
    }

    public bool EliminarVerificacion(int id)
    {
        var verificacion = ObtenerVerificacionPorId(id);

        if (verificacion == null)
            return false;

        return Verificaciones.Remove(verificacion);
    }

    public SolicitudOrientacion AgregarSolicitud(SolicitudOrientacion solicitud)
    {
        solicitud.IdSolicitud = _siguienteIdSolicitud++;
        Solicitudes.Add(solicitud);
        return solicitud;
    }

    public SolicitudOrientacion? ObtenerSolicitudPorId(int id)
    {
        return Solicitudes.FirstOrDefault(x => x.IdSolicitud == id);
    }

    public bool ActualizarSolicitud(SolicitudOrientacion solicitud)
    {
        var existente = ObtenerSolicitudPorId(solicitud.IdSolicitud);

        if (existente == null)
            return false;

        var indice = Solicitudes.IndexOf(existente);
        Solicitudes[indice] = solicitud;

        return true;
    }

    public bool EliminarSolicitud(int id)
    {
        var solicitud = ObtenerSolicitudPorId(id);

        if (solicitud == null)
            return false;

        return Solicitudes.Remove(solicitud);
    }
}
