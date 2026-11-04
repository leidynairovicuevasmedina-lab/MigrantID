
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class DocumentoFormPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public DocumentoFormPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarDocumentoEnEdicion();
    }

    private void CargarDocumentoEnEdicion()
    {
        Documento? documento =
            _servicio.DocumentoEnEdicion;

        if (documento == null)
        {
            TituloLabel.Text = "Registrar documento";
            LimpiarFormulario();
            return;
        }

        TituloLabel.Text = "Editar documento";

        TipoDocumentoPicker.SelectedItem =
            documento.TipoDocumento;

        NumeroDocumentoEntry.Text =
            documento.NumeroDocumento;

        PaisEmisionPicker.SelectedItem =
            documento.PaisEmision;

        FechaEmisionPicker.Date =
            documento.FechaEmision;

        FechaVencimientoPicker.Date =
            documento.FechaVencimiento;

        EstadoPicker.SelectedItem =
            documento.Estado;

        MensajeErrorLabel.IsVisible = false;
    }

    private async void OnGuardarClicked(
        object sender, EventArgs e)
    {
        string tipo =
            TipoDocumentoPicker.SelectedItem?.ToString()
            ?? "";

        string numero =
            NumeroDocumentoEntry.Text?.Trim() ?? "";

        string pais =
            PaisEmisionPicker.SelectedItem?.ToString()
            ?? "";

        string estado =
            EstadoPicker.SelectedItem?.ToString()
            ?? "";

        
        if (string.IsNullOrWhiteSpace(tipo) ||
            string.IsNullOrWhiteSpace(numero) ||
            string.IsNullOrWhiteSpace(pais) ||
            string.IsNullOrWhiteSpace(estado))
        {
            await MostrarError(
                "Completa todos los campos obligatorios.");
            return;
        }

        DateTime? fechaEmision =
            FechaEmisionPicker.Date;

        DateTime? fechaVencimiento =
            FechaVencimientoPicker.Date;

        if (!fechaEmision.HasValue ||
            !fechaVencimiento.HasValue)
        {
            await MostrarError(
                "Selecciona las fechas del documento.");
            return;
        }

        if (fechaEmision.Value.Date > DateTime.Today)
        {
            await MostrarError(
                "La fecha de emisión no puede ser futura.");
            return;
        }

        if (fechaVencimiento.Value.Date <=
            fechaEmision.Value.Date)
        {
            await MostrarError(
                "El vencimiento debe ser posterior " +
                "a la fecha de emisión.");
            return;
        }

        Documento? existente =
            _servicio.DocumentoEnEdicion;

        Documento documento = new()
        {
            IdDocumento =
                existente?.IdDocumento ?? 0,

            TipoDocumento = tipo,
            NumeroDocumento = numero,
            PaisEmision = pais,

            FechaEmision =
                fechaEmision.Value,

            FechaVencimiento =
                fechaVencimiento.Value,

            Estado = estado
        };

        
        if (existente == null)
        {
            _servicio.AgregarDocumento(documento);
        }
        else
        {
            _servicio.ActualizarDocumento(documento);
        }

        _servicio.DocumentoEnEdicion = null;

        await DisplayAlertAsync(
            "Operación completada",
            existente == null
                ? "Documento guardado correctamente."
                : "Documento actualizado correctamente.",
            "Aceptar");

        
        await Shell.Current.GoToAsync(
            "//documentos/lista-documentos");
    }

    private async void OnCancelarClicked(
        object sender, EventArgs e)
    {
        _servicio.DocumentoEnEdicion = null;

        await Shell.Current.GoToAsync(
            "//documentos/lista-documentos");
    }

    private async Task MostrarError(string mensaje)
    {
        MensajeErrorLabel.Text = mensaje;
        MensajeErrorLabel.IsVisible = true;

        await DisplayAlertAsync(
            "Revisa los datos",
            mensaje,
            "Aceptar");
    }

    private void LimpiarFormulario()
    {
        TipoDocumentoPicker.SelectedItem = null;

        NumeroDocumentoEntry.Text = "";

        PaisEmisionPicker.SelectedItem = null;

        FechaEmisionPicker.Date =
            DateTime.Today;

        FechaVencimientoPicker.Date =
            DateTime.Today.AddYears(1);

        EstadoPicker.SelectedItem =
            "En revisión";

        MensajeErrorLabel.Text = "";
        MensajeErrorLabel.IsVisible = false;
    }
}
