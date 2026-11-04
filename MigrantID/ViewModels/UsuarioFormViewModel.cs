
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

public partial class UsuarioFormViewModel : ObservableObject
{
    // SERVICIO EN MEMORIA

    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;


    // DATOS DEL FORMULARIO

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string correo = string.Empty;

    [ObservableProperty]
    private string? rolSeleccionado;


    // MENSAJES DE VALIDACION

    [ObservableProperty]
    private string nombreError = string.Empty;

    [ObservableProperty]
    private string correoError = string.Empty;

    [ObservableProperty]
    private string rolError = string.Empty;


    // ROLES DISPONIBLES

    public string[] RolesDisponibles { get; } =
    {
        "Usuario",
        "Agente",
        "Administrador"
    };


    // VALIDAR NOMBRE

    private void ValidarNombre()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            NombreError =
                "El nombre es obligatorio.";
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


    // VALIDAR CORREO

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


    // VALIDAR ROL

    private void ValidarRol()
    {
        if (string.IsNullOrWhiteSpace(RolSeleccionado))
        {
            RolError =
                "Debes seleccionar un rol.";
        }
        else
        {
            RolError = string.Empty;
        }
    }


    // VALIDAR FORMULARIO COMPLETO

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


    // GUARDAR USUARIO

    [RelayCommand]
    private async Task GuardarAsync()
    {
        // Validar todos los campos.
        if (!ValidarFormulario())
        {
            return;
        }

        // Crear usuario.
        Usuario nuevoUsuario = new Usuario
        {
            Nombre = Nombre.Trim(),
            Correo = Correo.Trim(),
            Rol = RolSeleccionado!,
            Activo = true
        };

        // Guardar en memoria.
        _servicio.AgregarUsuario(nuevoUsuario);

        // Mostrar confirmación.
        await Shell.Current.DisplayAlertAsync(
            "Registro exitoso",
            "El usuario fue registrado correctamente.",
            "Aceptar");

        // Limpiar campos y errores.
        LimpiarFormulario();

        // Regresar al listado.
        await Shell.Current.GoToAsync(
            "//usuarios/lista-usuarios");
    }


    // CANCELAR REGISTRO

    [RelayCommand]
    private async Task CancelarAsync()
    {
        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//usuarios/lista-usuarios");
    }


    // LIMPIAR FORMULARIO

    public void LimpiarFormulario()
    {
        Nombre = string.Empty;
        Correo = string.Empty;
        RolSeleccionado = null;

        NombreError = string.Empty;
        CorreoError = string.Empty;
        RolError = string.Empty;
    }
}
