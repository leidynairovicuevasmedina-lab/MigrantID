
using System;
using System.Linq;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class SolicitudesPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public SolicitudesPage()
    {
        InitializeComponent();

        SolicitudesCollection.ItemsSource =
            _servicio.Solicitudes;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        SolicitudesCollection.ItemsSource =
            _servicio.Solicitudes;
    }

    private async void OnAgregarClicked(
        object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//solicitud-form?id=0");
    }

    private async void OnSolicitudSeleccionada(
        object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault()
            is not SolicitudOrientacion solicitud)
        {
            return;
        }

        SolicitudesCollection.SelectedItem = null;

        await Shell.Current.GoToAsync(
            $"//solicitud-form?id={solicitud.IdSolicitud}");
    }

    private async void OnEditarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not SolicitudOrientacion solicitud)
        {
            return;
        }

        SolicitudesCollection.SelectedItem = null;

        await Shell.Current.GoToAsync(
            $"//solicitud-form?id={solicitud.IdSolicitud}");
    }

    private async void OnEliminarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not SolicitudOrientacion solicitud)
        {
            return;
        }

        SolicitudesCollection.SelectedItem = null;

        bool confirmar = await DisplayAlertAsync(
            "Eliminar solicitud",
            "¿Deseas eliminar esta solicitud?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        if (_servicio.EliminarSolicitud(
            solicitud.IdSolicitud))
        {
            await DisplayAlertAsync(
                "Solicitud eliminada",
                "El registro fue eliminado correctamente.",
                "Aceptar");
        }
        else
        {
            await DisplayAlertAsync(
                "No encontrada",
                "La solicitud ya no existe.",
                "Aceptar");
        }
    }
}
