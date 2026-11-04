
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.ViewModels;

public partial class SolicitudFormViewModel : ObservableObject
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    // USUARIOS DISPONIBLES

    public ObservableCollection<Usuario> UsuariosDisponibles
    { get; } = new();

    // TEMAS DISPONIBLES

    public string[] TemasDisponibles { get; } =
    {
        "Información migratoria",
        "Consulta de documentos",
        "Estado de solicitud",
        "Otra orientación"
    };

    // PROPIEDADES DEL FORMULARIO

    [ObservableProperty]
    private Usuario? usuarioSeleccionado;

    [ObservableProperty]
    private string? temaSeleccionado;

    [ObservableProperty]
    private string descripcion = string.Empty;

    // MENSAJES DE VALIDACIÓN

    [ObservableProperty]
    private string usuarioError = string.Empty;

    [ObservableProperty]
    private string temaError = string.Empty;

    [ObservableProperty]
    private string descripcionError = string.Empty;

    // ELIMINAR ERRORES AL CORREGIR CAMPOS

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

    // CARGAR USUARIOS REGISTRADOS

    public void CargarFormulario()
    {
        UsuariosDisponibles.Clear();

        foreach (Usuario usuario in _servicio.Usuarios)
        {
            UsuariosDisponibles.Add(usuario);
        }

        LimpiarFormulario();
    }

    // VALIDACIONES INDIVIDUALES

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        if (UsuarioSeleccionado == null)
        {
            UsuarioError =
                "Debe seleccionar un usuario.";
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

    // GUARDAR SOLICITUD

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!ValidarFormulario())
            return;

        SolicitudOrientacion nuevaSolicitud = new()
        {
            IdUsuario =
                UsuarioSeleccionado!.IdUsuario,

            Tema =
                TemaSeleccionado!,

            Descripcion =
                Descripcion.Trim(),

            FechaSolicitud =
                DateTime.Now,

            Estado =
                "Pendiente"
        };

        _servicio.AgregarSolicitud(nuevaSolicitud);

        await Shell.Current.DisplayAlertAsync(
            "Registro exitoso",
            "La solicitud fue registrada correctamente.",
            "Aceptar");

        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }

    // CANCELAR

    [RelayCommand]
    private async Task CancelarAsync()
    {
        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }

    // LIMPIAR MENSAJES DE ERROR

    private void LimpiarErrores()
    {
        UsuarioError = string.Empty;
        TemaError = string.Empty;
        DescripcionError = string.Empty;
    }

    // LIMPIAR FORMULARIO

    private void LimpiarFormulario()
    {
        UsuarioSeleccionado = null;
        TemaSeleccionado = null;
        Descripcion = string.Empty;

        LimpiarErrores();
    }
}
