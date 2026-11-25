
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class VerificacionesPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public VerificacionesPage()
    {
        InitializeComponent();

        VerificacionesCollection.ItemsSource =
            _servicio.Verificaciones;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CargarVerificaciones();
    }

    private void CargarVerificaciones()
    {
        VerificacionesCollection.ItemsSource =
            _servicio.Verificaciones;
    }

    private async void OnAgregarClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//verificacion-form?id=0");
    }

    private async void OnEditarClicked(
        object sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Verificacion verificacion)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"//verificacion-form?id={verificacion.IdVerificacion}");
    }

    private async void OnEliminarClicked(
        object sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Verificacion verificacion)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Eliminar verificación",
            "¿Deseas eliminar esta verificación?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
            return;

        bool eliminado = _servicio.EliminarVerificacion(
            verificacion.IdVerificacion);

        if (!eliminado)
        {
            await DisplayAlertAsync(
                "Error",
                "No se encontró la verificación.",
                "Aceptar");

            return;
        }

        await DisplayAlertAsync(
            "Verificación eliminada",
            "La verificación fue eliminada correctamente.",
            "Aceptar");
    }
}
