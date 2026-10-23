
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class SolicitudFormPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public SolicitudFormPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        
        UsuarioPicker.ItemsSource =
            _servicio.Usuarios;

        UsuarioPicker.SelectedIndex = -1;
        TemaPicker.SelectedIndex = -1;

        DescripcionEditor.Text = string.Empty;

        MensajeErrorLabel.IsVisible = false;
    }

    private async void OnGuardarClicked(
        object sender, EventArgs e)
    {
        MensajeErrorLabel.IsVisible = false;

        
        if (UsuarioPicker.SelectedItem
            is not Usuario usuario)
        {
            MostrarError(
                "Debe seleccionar un usuario.");

            return;
        }

        
        if (TemaPicker.SelectedIndex == -1)
        {
            MostrarError(
                "Debe seleccionar un tema.");

            return;
        }

        
        string descripcion =
            DescripcionEditor.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(descripcion))
        {
            MostrarError(
                "Debe escribir una descripción.");

            return;
        }

        
        SolicitudOrientacion nuevaSolicitud =
            new SolicitudOrientacion
            {
                IdUsuario =
                    usuario.IdUsuario,

                Tema =
                    TemaPicker.SelectedItem?.ToString()
                    ?? "",

                Descripcion = descripcion,

                FechaSolicitud =
                    DateTime.Now,

                Estado = "Pendiente"
            };

        
        _servicio.AgregarSolicitud(
            nuevaSolicitud);

        await DisplayAlertAsync(
            "Registro exitoso",
            "La solicitud fue registrada correctamente.",
            "Aceptar");

        
        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }

    private void MostrarError(string mensaje)
    {
        MensajeErrorLabel.Text = mensaje;
        MensajeErrorLabel.IsVisible = true;
    }

    private async void OnCancelarClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//solicitudes/lista-solicitudes");
    }
}
