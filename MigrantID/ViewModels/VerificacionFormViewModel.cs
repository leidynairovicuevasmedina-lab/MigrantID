
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.ViewModels;

public partial class VerificacionFormViewModel : ObservableObject
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    public ObservableCollection<Documento> DocumentosDisponibles
    { get; } = new();

    public string[] ResultadosDisponibles { get; } =
    {
        "Pendiente",
        "Verificado",
        "Requiere revisión"
    };

    [ObservableProperty]
    private Documento? documentoSeleccionado;

    [ObservableProperty]
    private DateTime fechaVerificacion = DateTime.Today;

    [ObservableProperty]
    private string? resultadoSeleccionado;

    [ObservableProperty]
    private string observacion = string.Empty;


    [ObservableProperty]
    private string documentoError = string.Empty;

    [ObservableProperty]
    private string fechaVerificacionError = string.Empty;

    [ObservableProperty]
    private string resultadoError = string.Empty;

    [ObservableProperty]
    private string observacionError = string.Empty;


    partial void OnDocumentoSeleccionadoChanged(Documento? value)
    {
        DocumentoError = string.Empty;
    }

    partial void OnFechaVerificacionChanged(DateTime value)
    {
        FechaVerificacionError = string.Empty;
    }

    partial void OnResultadoSeleccionadoChanged(string? value)
    {
        ResultadoError = string.Empty;
    }

    partial void OnObservacionChanged(string value)
    {
        ObservacionError = string.Empty;
    }


    public void CargarFormulario()
    {
        DocumentosDisponibles.Clear();

        foreach (Documento documento
                 in _servicio.ObtenerDocumentos())
        {
            DocumentosDisponibles.Add(documento);
        }

        LimpiarFormulario();
    }

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        if (DocumentoSeleccionado == null)
        {
            DocumentoError =
                "Debe seleccionar un documento.";
        }

        if (FechaVerificacion.Date > DateTime.Today)
        {
            FechaVerificacionError =
                "La fecha de verificación no puede ser futura.";
        }

        if (string.IsNullOrWhiteSpace(ResultadoSeleccionado))
        {
            ResultadoError =
                "Debe seleccionar un resultado.";
        }

        if (string.IsNullOrWhiteSpace(Observacion))
        {
            ObservacionError =
                "Debe escribir una observación.";
        }

        return new[]
        {
            DocumentoError,
            FechaVerificacionError,
            ResultadoError,
            ObservacionError
        }.All(string.IsNullOrEmpty);
    }


    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!ValidarFormulario())
            return;

        Verificacion nuevaVerificacion = new()
        {
            IdDocumento =
                DocumentoSeleccionado!.IdDocumento,

            FechaVerificacion =
                FechaVerificacion,

            Resultado =
                ResultadoSeleccionado!,

            Observacion =
                Observacion.Trim()
        };

        _servicio.AgregarVerificacion(nuevaVerificacion);

        await Shell.Current.DisplayAlertAsync(
            "Registro exitoso",
            "La verificación fue registrada correctamente.",
            "Aceptar");

        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//verificaciones/lista-verificaciones");
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//verificaciones/lista-verificaciones");
    }

    private void LimpiarErrores()
    {
        DocumentoError = string.Empty;
        FechaVerificacionError = string.Empty;
        ResultadoError = string.Empty;
        ObservacionError = string.Empty;
    }

    private void LimpiarFormulario()
    {
        DocumentoSeleccionado = null;
        FechaVerificacion = DateTime.Today;
        ResultadoSeleccionado = null;
        Observacion = string.Empty;

        LimpiarErrores();
    }
}
