
using MigrantID.Services;

namespace MigrantID.Views;

public partial class SolicitudesPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public SolicitudesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CargarSolicitudes();
    }

    // CARGAR SOLICITUDES REGISTRADAS

    private void CargarSolicitudes()
    {
        var solicitudes = _servicio.Solicitudes;

        SolicitudesCollection.ItemsSource = null;

        SolicitudesCollection.ItemsSource = solicitudes;
    }

    // AGREGAR NUEVA SOLICITUD

    private async void OnAgregarClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//solicitud-form");
    }
}
