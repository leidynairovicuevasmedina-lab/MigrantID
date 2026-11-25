
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.ViewModels;

public partial class UsuarioFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly MigrantIDService _servicio = MigrantIDService.Instancia;

    private int _idEditando;

    [ObservableProperty]
    private bool modoEditar;

    [ObservableProperty]
    private string tituloPagina = "Registrar usuario";

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string correo = string.Empty;

    [ObservableProperty]
    private string? rolSeleccionado;

    [ObservableProperty]
    private string nombreError = string.Empty;

    [ObservableProperty]
    private string correoError = string.Empty;

    [ObservableProperty]
    private string rolError = string.Empty;

    public string[] RolesDisponibles { get; } =
    {
        "Usuario",
        "Agente",
        "Administrador"
    };

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var valor) &&
            int.TryParse(valor?.ToString(), out int id))
        {
            Usuario? usuario = _servicio.ObtenerUsuarioPorId(id);

            if (usuario != null)
            {
                _idEditando = usuario.IdUsuario;
                ModoEditar = true;
                TituloPagina = "Editar usuario";

                Nombre = usuario.Nombre;
                Correo = usuario.Correo;
                RolSeleccionado = usuario.Rol;

                LimpiarErrores();
                return;
            }
        }

        LimpiarFormulario();
    }

    partial void OnNombreChanged(string value)
    {
        NombreError = string.Empty;
    }

    partial void OnCorreoChanged(string value)
    {
        CorreoError = string.Empty;
    }

    partial void OnRolSeleccionadoChanged(string? value)
    {
        RolError = string.Empty;
    }

    private void ValidarNombre()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            NombreError = "El nombre es obligatorio.";
        }
        else if (Nombre.Trim().Length < 2 ||
                 Nombre.Trim().Length > 100)
        {
            NombreError = "El nombre debe tener entre 2 y 100 caracteres.";
        }
        else if (!Regex.IsMatch(Nombre.Trim(), @"^[\p{L}\s'-]+$"))
        {
            NombreError = "El nombre solo puede contener letras y espacios.";
        }
        else
        {
            NombreError = string.Empty;
        }
    }

    private void ValidarCorreo()
    {
        if (string.IsNullOrWhiteSpace(Correo))
        {
            CorreoError = "El correo electrónico es obligatorio.";
        }
        else if (!Regex.IsMatch(Correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            CorreoError = "Introduce un correo electrónico válido.";
        }
        else
        {
            CorreoError = string.Empty;
        }
    }

    private void ValidarRol()
    {
        RolError = string.IsNullOrWhiteSpace(RolSeleccionado)
            ? "Debes seleccionar un rol."
            : string.Empty;
    }

    private bool ValidarFormulario()
    {
        ValidarNombre();
        ValidarCorreo();
        ValidarRol();

        return string.IsNullOrEmpty(NombreError) &&
               string.IsNullOrEmpty(CorreoError) &&
               string.IsNullOrEmpty(RolError);
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!ValidarFormulario())
            return;

        Usuario usuario = new()
        {
            IdUsuario = _idEditando,
            Nombre = Nombre.Trim(),
            Correo = Correo.Trim(),
            Rol = RolSeleccionado!,
            Activo = true
        };

        bool actualizando = ModoEditar;

        if (actualizando)
        {
            Usuario? anterior = _servicio.ObtenerUsuarioPorId(_idEditando);

            if (anterior == null)
                return;

            usuario.Activo = anterior.Activo;

            if (!_servicio.ActualizarUsuario(usuario))
                return;
        }
        else
        {
            _servicio.AgregarUsuario(usuario);
        }

        await Shell.Current.DisplayAlertAsync(
            "Operación completada",
            actualizando
                ? "El usuario fue actualizado correctamente."
                : "El usuario fue registrado correctamente.",
            "Aceptar");

        LimpiarFormulario();

        await Shell.Current.GoToAsync("//usuarios/lista-usuarios");
    }

    [RelayCommand]
    private async Task EliminarAsync()
    {
        if (!ModoEditar)
            return;

        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Eliminar usuario",
            $"¿Deseas eliminar a {Nombre}?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
            return;

        _servicio.EliminarUsuario(_idEditando);

        LimpiarFormulario();

        await Shell.Current.DisplayAlertAsync(
            "Usuario eliminado",
            "El usuario fue eliminado correctamente.",
            "Aceptar");

        await Shell.Current.GoToAsync("//usuarios/lista-usuarios");
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        LimpiarFormulario();
        await Shell.Current.GoToAsync("//usuarios/lista-usuarios");
    }

    private void LimpiarErrores()
    {
        NombreError = string.Empty;
        CorreoError = string.Empty;
        RolError = string.Empty;
    }

    public void LimpiarFormulario()
    {
        _idEditando = 0;
        ModoEditar = false;
        TituloPagina = "Registrar usuario";

        Nombre = string.Empty;
        Correo = string.Empty;
        RolSeleccionado = null;

        LimpiarErrores();
    }
}
