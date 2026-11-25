
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.ViewModels;

public partial class VerificacionFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    private int _idEditando;

    [ObservableProperty]
    private bool modoEditar;

    [ObservableProperty]
    private string tituloPagina = "Registrar verificación";

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

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        CargarFormulario();

        if (query.TryGetValue("id", out var valor) &&
            int.TryParse(valor?.ToString(), out int id) &&
            id > 0)
        {
            Verificacion? verificacion =
                _servicio.ObtenerVerificacionPorId(id);

            if (verificacion != null)
            {
                CargarVerificacion(verificacion);
                return;
            }
        }

        LimpiarFormulario();
    }

    public void CargarFormulario()
    {
        int? idDocumento =
            DocumentoSeleccionado?.IdDocumento;

        DocumentosDisponibles.Clear();

        foreach (Documento documento in _servicio.Documentos)
        {
            DocumentosDisponibles.Add(documento);
        }

        if (idDocumento.HasValue)
        {
            DocumentoSeleccionado =
                DocumentosDisponibles.FirstOrDefault(
                    x => x.IdDocumento == idDocumento.Value);
        }
    }

    private void CargarVerificacion(Verificacion verificacion)
    {
        _idEditando = verificacion.IdVerificacion;

        ModoEditar = true;
        TituloPagina = "Editar verificación";

        DocumentoSeleccionado =
            DocumentosDisponibles.FirstOrDefault(
                x => x.IdDocumento == verificacion.IdDocumento);

        FechaVerificacion = verificacion.FechaVerificacion;
        ResultadoSeleccionado = verificacion.Resultado;
        Observacion = verificacion.Observacion;

        LimpiarErrores();
    }

    partial void OnDocumentoSeleccionadoChanged(
        Documento? value)
    {
        DocumentoError = string.Empty;
    }

    partial void OnFechaVerificacionChanged(
        DateTime value)
    {
        FechaVerificacionError = string.Empty;
    }

    partial void OnResultadoSeleccionadoChanged(
        string? value)
    {
        ResultadoError = string.Empty;
    }

    partial void OnObservacionChanged(
        string value)
    {
        ObservacionError = string.Empty;
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

        Verificacion verificacion = new()
        {
            IdVerificacion = _idEditando,
            IdDocumento = DocumentoSeleccionado!.IdDocumento,
            FechaVerificacion = FechaVerificacion,
            Resultado = ResultadoSeleccionado!,
            Observacion = Observacion.Trim()
        };

        bool actualizando = ModoEditar;

        if (actualizando)
        {
            bool actualizado =
                _servicio.ActualizarVerificacion(verificacion);

            if (!actualizado)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Error",
                    "No se encontró la verificación.",
                    "Aceptar");

                return;
            }
        }
        else
        {
            _servicio.AgregarVerificacion(verificacion);
        }

        await Shell.Current.DisplayAlertAsync(
            "Operación completada",
            actualizando
                ? "La verificación fue actualizada correctamente."
                : "La verificación fue registrada correctamente.",
            "Aceptar");

        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//verificaciones/lista-verificaciones");
    }

    [RelayCommand]
    private async Task EliminarAsync()
    {
        if (!ModoEditar)
            return;

        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Eliminar verificación",
            "¿Deseas eliminar esta verificación?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
            return;

        bool eliminado =
            _servicio.EliminarVerificacion(_idEditando);

        if (!eliminado)
        {
            await Shell.Current.DisplayAlertAsync(
                "Error",
                "No se encontró la verificación.",
                "Aceptar");

            return;
        }

        LimpiarFormulario();

        await Shell.Current.DisplayAlertAsync(
            "Verificación eliminada",
            "La verificación fue eliminada correctamente.",
            "Aceptar");

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
        _idEditando = 0;

        ModoEditar = false;
        TituloPagina = "Registrar verificación";

        DocumentoSeleccionado = null;
        FechaVerificacion = DateTime.Today;
        ResultadoSeleccionado = null;
        Observacion = string.Empty;

        LimpiarErrores();
    }
}
