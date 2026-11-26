
using System;
using System.Linq;
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

        VerificacionesCollection.ItemsSource =
            _servicio.Verificaciones;
    }

    private async void OnAgregarClicked(
        object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//verificacion-form?id=0");
    }

    private async void OnVerificacionSeleccionada(
        object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault()
            is not Verificacion verificacion)
        {
            return;
        }

        VerificacionesCollection.SelectedItem = null;

        await Shell.Current.GoToAsync(
            $"//verificacion-form?id={verificacion.IdVerificacion}");
    }

    private async void OnEditarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Verificacion verificacion)
        {
            return;
        }

        VerificacionesCollection.SelectedItem = null;

        await Shell.Current.GoToAsync(
            $"//verificacion-form?id={verificacion.IdVerificacion}");
    }

    private async void OnEliminarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Verificacion verificacion)
        {
            return;
        }

        VerificacionesCollection.SelectedItem = null;

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
