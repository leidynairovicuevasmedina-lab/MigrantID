
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;
using Microsoft.Maui.Controls;

namespace MigrantID.ViewModels;

public partial class SolicitudFormViewModel :
    ObservableObject, IQueryAttributable
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    private int _idEditando;

    
    public ObservableCollection<Usuario> UsuariosDisponibles
    { get; } = new();

    
    public string[] TemasDisponibles { get; } =
    {
        "Información migratoria",
        "Consulta de documentos",
        "Estado de solicitud",
        "Otra orientación"
    };

    
    [ObservableProperty]
    private bool modoEditar;

    [ObservableProperty]
    private string tituloPagina =
        "Registrar solicitud de orientación";

    
    [ObservableProperty]
    private Usuario? usuarioSeleccionado;

    [ObservableProperty]
    private string? temaSeleccionado;

    [ObservableProperty]
    private string descripcion = string.Empty;

    
    [ObservableProperty]
    private string usuarioError = string.Empty;

    [ObservableProperty]
    private string temaError = string.Empty;

    [ObservableProperty]
    private string descripcionError = string.Empty;


    
    partial void OnUsuarioSeleccionadoChanged(Usuario? value)
    {
        UsuarioError = string.Empty;
    }

    partial void OnTemaSeleccionadoChanged(string? value)
    {
        TemaError = string.Empty;
    }

    partial void OnDescripcionChanged(string value)
    {
        DescripcionError = string.Empty;
    }


    
    public void CargarFormulario()
    {
        int? idSeleccionado =
            UsuarioSeleccionado?.IdUsuario;

        UsuariosDisponibles.Clear();

        foreach (var usuario in _servicio.Usuarios)
        {
            UsuariosDisponibles.Add(usuario);
        }

        if (idSeleccionado.HasValue)
        {
            UsuarioSeleccionado =
                UsuariosDisponibles.FirstOrDefault(
                    x => x.IdUsuario == idSeleccionado.Value);
        }
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        CargarFormulario();

        if (query.TryGetValue("id", out var valor) &&
            int.TryParse(valor?.ToString(), out int id) &&
            id > 0)
        {
            var solicitud =
                _servicio.ObtenerSolicitudPorId(id);

            if (solicitud is not null)
            {
                CargarDesde(solicitud);
                return;
            }
        }

        PrepararNueva();
    }


    
    private void CargarDesde(SolicitudOrientacion solicitud)
    {
        _idEditando = solicitud.IdSolicitud;

        ModoEditar = true;

        TituloPagina =
            "Editar solicitud de orientación";

        UsuarioSeleccionado =
            UsuariosDisponibles.FirstOrDefault(
                x => x.IdUsuario == solicitud.IdUsuario);

        TemaSeleccionado = solicitud.Tema;
        Descripcion = solicitud.Descripcion;

        LimpiarErrores();
    }


    
    private void PrepararNueva()
    {
        _idEditando = 0;
        ModoEditar = false;

        TituloPagina =
            "Registrar solicitud de orientación";

        UsuarioSeleccionado = null;
        TemaSeleccionado = null;
        Descripcion = string.Empty;

        LimpiarErrores();
    }


    
    private void LimpiarErrores()
    {
        UsuarioError = string.Empty;
        TemaError = string.Empty;
        DescripcionError = string.Empty;
    }


    
    private bool ValidarFormulario()
    {
        LimpiarErrores();

        if (UsuarioSeleccionado is null ||
            _servicio.ObtenerUsuarioPorId(
                UsuarioSeleccionado.IdUsuario) is null)
        {
            UsuarioError =
                "Debe seleccionar un usuario válido.";
        }

        if (string.IsNullOrWhiteSpace(TemaSeleccionado))
        {
            TemaError =
                "Debe seleccionar un tema.";
        }

        if (string.IsNullOrWhiteSpace(Descripcion))
        {
            DescripcionError =
                "Debe escribir una descripción.";
        }

        return new[]
        {
            UsuarioError,
            TemaError,
            DescripcionError
        }.All(string.IsNullOrEmpty);
    }


    
    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!ValidarFormulario())
        {
            return;
        }

        SolicitudOrientacion? anterior = ModoEditar
            ? _servicio.ObtenerSolicitudPorId(_idEditando)
            : null;

        if (ModoEditar && anterior is null)
        {
            await Shell.Current.DisplayAlertAsync(
                "No encontrada",
                "La solicitud que intentas editar ya no existe.",
                "Aceptar");

            return;
        }

        SolicitudOrientacion registro = new()
        {
            IdSolicitud = _idEditando,
            IdUsuario = UsuarioSeleccionado!.IdUsuario,
            Tema = TemaSeleccionado!,
            Descripcion = Descripcion.Trim(),

            FechaSolicitud =
                anterior?.FechaSolicitud ?? DateTime.Now,

            Estado =
                anterior?.Estado ?? "Pendiente"
        };

        bool editar = ModoEditar;

        if (editar)
        {
            if (!_servicio.ActualizarSolicitud(registro))
            {
                return;
            }
        }
        else
        {
            _servicio.AgregarSolicitud(registro);
        }

        await Shell.Current.DisplayAlertAsync(
            "Operación completada",
            editar
                ? "Solicitud actualizada correctamente."
                : "Solicitud registrada correctamente.",
            "Aceptar");

        PrepararNueva();

        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }


   
    [RelayCommand]
    private async Task EliminarAsync()
    {
        if (!ModoEditar)
        {
            return;
        }

        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Eliminar solicitud",
            "¿Deseas eliminar esta solicitud?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        if (!_servicio.EliminarSolicitud(_idEditando))
        {
            await Shell.Current.DisplayAlertAsync(
                "No encontrada",
                "La solicitud ya no existe.",
                "Aceptar");

            return;
        }

        await Shell.Current.DisplayAlertAsync(
            "Solicitud eliminada",
            "El registro fue eliminado correctamente.",
            "Aceptar");

        PrepararNueva();

        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }


 
    [RelayCommand]
    private async Task CancelarAsync()
    {
        PrepararNueva();

        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }
}
