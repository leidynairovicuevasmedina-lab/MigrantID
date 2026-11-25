
using System;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class SolicitudesPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    // CONSTRUCTOR

    public SolicitudesPage()
    {
        InitializeComponent();

        SolicitudesCollection.ItemsSource =
            _servicio.Solicitudes;
    }

    // MOSTRAR SOLICITUDES

    protected override void OnAppearing()
    {
        base.OnAppearing();

        SolicitudesCollection.ItemsSource =
            _servicio.Solicitudes;
    }

    // AGREGAR

    private async void OnAgregarClicked(
        object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//solicitud-form?id=0");
    }

    // EDITAR

    private async void OnEditarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not SolicitudOrientacion solicitud)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"//solicitud-form?id={solicitud.IdSolicitud}");
    }

    // ELIMINAR

    private async void OnEliminarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not SolicitudOrientacion solicitud)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Eliminar solicitud",
            "¿Deseas eliminar esta solicitud?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        if (_servicio.EliminarSolicitud(solicitud.IdSolicitud))
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
