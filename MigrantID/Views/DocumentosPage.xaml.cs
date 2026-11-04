
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

    // MOSTRAR DOCUMENTOS

    private void CargarDocumentos()
    {
        var documentos = _servicio.ObtenerDocumentos();

        DocumentosCollection.ItemsSource = null;
        DocumentosCollection.ItemsSource = documentos;
    }

    // AGREGAR DOCUMENTO

    private async void OnAgregarClicked(
        object sender, EventArgs e)
    {
        _servicio.DocumentoEnEdicion = null;

        await Shell.Current.GoToAsync(
            "//documento-form");
    }

    // EDITAR DOCUMENTO

    private async void OnEditarClicked(
        object sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Documento documento)
        {
            return;
        }

        _servicio.DocumentoEnEdicion = documento;

        await Shell.Current.GoToAsync(
            "//documento-form");
    }

    // ELIMINAR DOCUMENTO

    private async void OnEliminarClicked(
        object sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Documento documento)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Eliminar documento",
            $"¿Deseas eliminar el documento {documento.NumeroDocumento}?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        _servicio.EliminarDocumento(
            documento.IdDocumento);

        CargarDocumentos();

        await DisplayAlertAsync(
            "Documento eliminado",
            "El documento se ha eliminado correctamente.",
            "Aceptar");
    }
}
