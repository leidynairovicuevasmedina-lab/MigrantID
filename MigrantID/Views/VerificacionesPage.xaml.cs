
using System;
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

    // CARGAR VERIFICACIONES

    protected override void OnAppearing()
    {
        base.OnAppearing();

        VerificacionesCollection.ItemsSource =
            _servicio.Verificaciones;
    }

    // AGREGAR

    private async void OnAgregarClicked(
        object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//verificacion-form?id=0");
    }

    // EDITAR

    private async void OnEditarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Verificacion verificacion)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"//verificacion-form?id={verificacion.IdVerificacion}");
    }

    // ELIMINAR

    private async void OnEliminarClicked(
        object? sender, EventArgs e)
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
        {
            return;
        }

        if (_servicio.EliminarVerificacion(
            verificacion.IdVerificacion))
        {
            await DisplayAlertAsync(
                "Verificación eliminada",
                "El registro fue eliminado correctamente.",
                "Aceptar");
        }
        else
        {
            await DisplayAlertAsync(
                "No encontrada",
                "La verificación ya no existe.",
                "Aceptar");
        }
    }
}
