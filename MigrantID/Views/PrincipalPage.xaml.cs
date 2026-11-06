
using System;
using Microsoft.Maui.Controls;
using MigrantID.Services;

namespace MigrantID.Views
{
    public partial class PrincipalPage : ContentPage
    {
        private readonly MigrantIDService _servicio =
            MigrantIDService.Instancia;

        public PrincipalPage()
        {
            InitializeComponent();

            ActualizarDashboard();
        }

        // Se ejecuta cada vez que entramos a Inicio.

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ActualizarDashboard();
        }

        // Actualiza los cuatro indicadores
        // utilizando los registros reales.

        private void ActualizarDashboard()
        {
            ContadorDocumentos.Text =
                _servicio.Documentos.Count.ToString();

            ContadorUsuarios.Text =
                _servicio.Usuarios.Count.ToString();

            ContadorVerificaciones.Text =
                _servicio.Verificaciones.Count.ToString();

            ContadorSolicitudes.Text =
                _servicio.Solicitudes.Count.ToString();
        }

        // NAVEGAR A DOCUMENTOS

        private async void OnDocumentosClicked(
            object? sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync("//documentos");
        }

        // NAVEGAR A USUARIOS

        private async void OnUsuariosClicked(
            object? sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync("//usuarios");
        }

        // NAVEGAR A VERIFICACIONES

        private async void OnVerificacionesClicked(
            object? sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync("//verificaciones");
        }

        // NAVEGAR A SOLICITUDES

        private async void OnSolicitudesClicked(
            object? sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync("//solicitudes");
        }
    }
}
