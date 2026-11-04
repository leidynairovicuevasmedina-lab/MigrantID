
using MigrantID.Services;

namespace MigrantID.Views;

public partial class VerificacionesPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public VerificacionesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CargarVerificaciones();
    }

    // MOSTRAR VERIFICACIONES REGISTRADAS

    private void CargarVerificaciones()
    {
        var verificaciones = _servicio.Verificaciones;

        VerificacionesCollection.ItemsSource = null;

        VerificacionesCollection.ItemsSource =
            verificaciones;
    }

    // AGREGAR VERIFICACIÓN

    private async void OnAgregarClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//verificacion-form");
    }
}
