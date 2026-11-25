
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;
using Microsoft.Maui.Controls;

namespace MigrantID.ViewModels;

public partial class UsuarioFormViewModel :
    ObservableObject, IQueryAttributable
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

  
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

 
    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var valor) &&
            int.TryParse(valor?.ToString(), out int id) &&
            id > 0)
        {
            Usuario? usuario =
                _servicio.ObtenerUsuarioPorId(id);

            if (usuario != null)
            {
                CargarUsuario(usuario);
                return;
            }
        }

        PrepararNuevoUsuario();
    }

    
    private void CargarUsuario(Usuario usuario)
    {
        _idEditando = usuario.IdUsuario;

        ModoEditar = true;
        TituloPagina = "Editar usuario";

        Nombre = usuario.Nombre;
        Correo = usuario.Correo;
        RolSeleccionado = usuario.Rol;

        LimpiarErrores();
    }

    
    private void PrepararNuevoUsuario()
    {
        _idEditando = 0;

        ModoEditar = false;
        TituloPagina = "Registrar usuario";

        LimpiarFormulario();
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
            NombreError =
                "El nombre debe contener entre 2 y 100 caracteres.";
        }
        else if (!Regex.IsMatch(
            Nombre.Trim(),
            @"^[\p{L}\s'-]+$"))
        {
            NombreError =
                "El nombre solo puede contener letras y espacios.";
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
            CorreoError =
                "El correo electrónico es obligatorio.";
        }
        else if (!Regex.IsMatch(
            Correo.Trim(),
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            CorreoError =
                "Introduce un correo electrónico válido.";
        }
        else
        {
            CorreoError = string.Empty;
        }
    }

    
    private void ValidarRol()
    {
        if (string.IsNullOrWhiteSpace(RolSeleccionado))
        {
            RolError = "Debes seleccionar un rol.";
        }
        else
        {
            RolError = string.Empty;
        }
    }

    
    private bool ValidarFormulario()
    {
        ValidarNombre();
        ValidarCorreo();
        ValidarRol();

        return
            string.IsNullOrEmpty(NombreError) &&
            string.IsNullOrEmpty(CorreoError) &&
            string.IsNullOrEmpty(RolError);
    }

    
    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!ValidarFormulario())
        {
            return;
        }

        if (ModoEditar)
        {
            Usuario? existente =
                _servicio.ObtenerUsuarioPorId(_idEditando);

            if (existente == null)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Usuario no encontrado",
                    "No se encontró el usuario para actualizar.",
                    "Aceptar");

                return;
            }

            Usuario usuarioActualizado = new()
            {
                IdUsuario = existente.IdUsuario,
                Nombre = Nombre.Trim(),
                Correo = Correo.Trim(),
                Rol = RolSeleccionado!,
                Activo = existente.Activo
            };

            bool actualizado =
                _servicio.ActualizarUsuario(usuarioActualizado);

            if (!actualizado)
            {
                return;
            }

            await Shell.Current.DisplayAlertAsync(
                "Usuario actualizado",
                "Los datos del usuario se actualizaron correctamente.",
                "Aceptar");
        }
        else
        {
            Usuario nuevoUsuario = new()
            {
                Nombre = Nombre.Trim(),
                Correo = Correo.Trim(),
                Rol = RolSeleccionado!,
                Activo = true
            };

            _servicio.AgregarUsuario(nuevoUsuario);

            await Shell.Current.DisplayAlertAsync(
                "Registro exitoso",
                "El usuario fue registrado correctamente.",
                "Aceptar");
        }

        PrepararNuevoUsuario();

        await Shell.Current.GoToAsync(
            "//usuarios/lista-usuarios");
    }

    
    [RelayCommand]
    private async Task EliminarAsync()
    {
        if (!ModoEditar || _idEditando <= 0)
        {
            return;
        }

        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Eliminar usuario",
            $"¿Deseas eliminar a {Nombre}?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        bool eliminado =
            _servicio.EliminarUsuario(_idEditando);

        if (!eliminado)
        {
            await Shell.Current.DisplayAlertAsync(
                "Usuario no encontrado",
                "No fue posible eliminar el usuario.",
                "Aceptar");

            return;
        }

        await Shell.Current.DisplayAlertAsync(
            "Usuario eliminado",
            "El usuario fue eliminado correctamente.",
            "Aceptar");

        PrepararNuevoUsuario();

        await Shell.Current.GoToAsync(
            "//usuarios/lista-usuarios");
    }

    
    [RelayCommand]
    private async Task CancelarAsync()
    {
        PrepararNuevoUsuario();

        await Shell.Current.GoToAsync(
            "//usuarios/lista-usuarios");
    }

    
    public void LimpiarFormulario()
    {
        Nombre = string.Empty;
        Correo = string.Empty;
        RolSeleccionado = null;

        LimpiarErrores();
    }

    
    private void LimpiarErrores()
    {
        NombreError = string.Empty;
        CorreoError = string.Empty;
        RolError = string.Empty;
    }
}
