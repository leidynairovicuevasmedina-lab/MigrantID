
using System;
using System.Linq;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class DocumentosPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public DocumentosPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarDocumentos();
    }

    private void CargarDocumentos()
    {
        var documentos = _servicio.ObtenerDocumentos();

        DocumentosCollection.ItemsSource = null;
        DocumentosCollection.ItemsSource = documentos;
    }

    private async void OnAgregarClicked(
        object? sender, EventArgs e)
    {
        _servicio.DocumentoEnEdicion = null;

        await Shell.Current.GoToAsync(
            "//documento-form");
    }

    private async void OnDocumentoSeleccionado(
        object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault()
            is not Documento documento)
        {
            return;
        }

        DocumentosCollection.SelectedItem = null;

        _servicio.DocumentoEnEdicion = documento;

        await Shell.Current.GoToAsync(
            "//documento-form");
    }

    private async void OnEditarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Documento documento)
        {
            return;
        }

        DocumentosCollection.SelectedItem = null;

        _servicio.DocumentoEnEdicion = documento;

        await Shell.Current.GoToAsync(
            "//documento-form");
    }

    private async void OnEliminarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Documento documento)
        {
            return;
        }

        DocumentosCollection.SelectedItem = null;

        bool confirmar = await DisplayAlertAsync(
            "Eliminar documento",
            $"¿Deseas eliminar el documento {documento.NumeroDocumento}?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        bool eliminado =
            _servicio.EliminarDocumento(documento.IdDocumento);

        if (eliminado)
        {
            CargarDocumentos();

            await DisplayAlertAsync(
                "Documento eliminado",
                "El documento se ha eliminado correctamente.",
                "Aceptar");
        }
        else
        {
            await DisplayAlertAsync(
                "Error",
                "No se encontró el documento seleccionado.",
                "Aceptar");
        }
    }
}
