
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class VerificacionFormPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public VerificacionFormPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        
        DocumentoPicker.ItemsSource =
            _servicio.ObtenerDocumentos();

        DocumentoPicker.SelectedIndex = -1;
        ResultadoPicker.SelectedIndex = -1;
        ObservacionEditor.Text = string.Empty;

        FechaVerificacionPicker.Date =
            DateTime.Today;

        MensajeErrorLabel.IsVisible = false;
    }

    private async void OnGuardarClicked(
        object sender, EventArgs e)
    {
        MensajeErrorLabel.IsVisible = false;

        
        if (DocumentoPicker.SelectedItem
            is not Documento documento)
        {
            MostrarError(
                "Debe seleccionar un documento.");

            return;
        }

        
        if (ResultadoPicker.SelectedIndex == -1)
        {
            MostrarError(
                "Debe seleccionar un resultado.");

            return;
        }

        string resultado =
            ResultadoPicker.SelectedItem?.ToString()
            ?? "";

        string observacion =
            ObservacionEditor.Text?.Trim() ?? "";

        
        if (string.IsNullOrWhiteSpace(observacion))
        {
            MostrarError(
                "Debe escribir una observación.");

            return;
        }

        
        Verificacion nuevaVerificacion =
            new Verificacion
            {
                IdDocumento =
                    documento.IdDocumento,

                FechaVerificacion =
                    FechaVerificacionPicker.Date
                    ?? DateTime.Today,

                Resultado = resultado,

                Observacion = observacion
            };

        
        _servicio.AgregarVerificacion(
            nuevaVerificacion);

        await DisplayAlertAsync(
            "Registro exitoso",
            "La verificación fue registrada correctamente.",
            "Aceptar");

        
        await Shell.Current.GoToAsync(
            "//verificaciones/lista-verificaciones");
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
            "//verificaciones/lista-verificaciones");
    }
}
