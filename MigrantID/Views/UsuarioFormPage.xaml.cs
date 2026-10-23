
using System;
using System.Linq;
using Microsoft.Maui.Controls;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views
{
    [QueryProperty(nameof(IdUsuario), "id")]
    public partial class UsuarioFormPage : ContentPage
    {
        private readonly MigrantIDService _servicio =
            MigrantIDService.Instancia;

        private Usuario? _usuarioEnEdicion;

        public string IdUsuario
        {
            set
            {
                if (int.TryParse(value, out int id))
                {
                    _usuarioEnEdicion =
                        _servicio.Usuarios.FirstOrDefault(
                            u => u.IdUsuario == id);
                }
                else
                {
                    _usuarioEnEdicion = null;
                }
            }
        }

        public UsuarioFormPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MensajeErrorLabel.IsVisible = false;

            if (_usuarioEnEdicion != null)
            {
                TituloLabel.Text = "Editar usuario";
                GuardarButton.Text = "GUARDAR CAMBIOS";

                NombreEntry.Text = _usuarioEnEdicion.Nombre;
                CorreoEntry.Text = _usuarioEnEdicion.Correo;

                RolPicker.SelectedItem = _usuarioEnEdicion.Rol;
            }
            else
            {
                TituloLabel.Text = "Registrar usuario";
                GuardarButton.Text = "GUARDAR";

                NombreEntry.Text = string.Empty;
                CorreoEntry.Text = string.Empty;
                RolPicker.SelectedIndex = -1;
            }
        }

        private async void OnGuardarClicked(
            object sender, EventArgs e)
        {
            string nombre = NombreEntry.Text?.Trim() ?? "";
            string correo = CorreoEntry.Text?.Trim() ?? "";
            string rol = RolPicker.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(rol))
            {
                MensajeErrorLabel.Text =
                    "Debe completar todos los campos.";

                MensajeErrorLabel.IsVisible = true;
                return;
            }

            if (!correo.Contains("@") || !correo.Contains("."))
            {
                MensajeErrorLabel.Text =
                    "Ingrese un correo electrónico válido.";

                MensajeErrorLabel.IsVisible = true;
                return;
            }

            if (_usuarioEnEdicion == null)
            {
                Usuario nuevoUsuario = new Usuario
                {
                    IdUsuario = _servicio.Usuarios.Count == 0
                        ? 1
                        : _servicio.Usuarios.Max(u => u.IdUsuario) + 1,
                    Nombre = nombre,
                    Correo = correo,
                    Rol = rol,
                    Activo = true
                };

                _servicio.Usuarios.Add(nuevoUsuario);
            }
            else
            {
                _usuarioEnEdicion.Nombre = nombre;
                _usuarioEnEdicion.Correo = correo;
                _usuarioEnEdicion.Rol = rol;
            }

            await DisplayAlertAsync(
                "Usuario guardado",
                "Los datos se guardaron correctamente.",
                "Aceptar");

            await Shell.Current.GoToAsync("//usuarios");
        }

        private async void OnCancelarClicked(
            object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//usuarios");
        }
    }
}
