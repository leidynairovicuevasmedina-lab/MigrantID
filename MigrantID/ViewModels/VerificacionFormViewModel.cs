
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;
using Microsoft.Maui.Controls;

namespace MigrantID.ViewModels;

public partial class VerificacionFormViewModel :
    ObservableObject, IQueryAttributable
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    private int _idEditando;

    public ObservableCollection<Documento> DocumentosDisponibles
    { get; } = new();

    public string[] ResultadosDisponibles { get; } =
    {
        "Pendiente",
        "Verificado",
        "Requiere revisión"
    };

    

    [ObservableProperty]
    private bool modoEditar;

    [ObservableProperty]
    private string tituloPagina = "Registrar verificación";

    
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
        int? idSeleccionado = DocumentoSeleccionado?.IdDocumento;

        DocumentosDisponibles.Clear();

        foreach (var documento in _servicio.ObtenerDocumentos())
        {
            DocumentosDisponibles.Add(documento);
        }

        if (idSeleccionado.HasValue)
        {
            DocumentoSeleccionado =
                DocumentosDisponibles.FirstOrDefault(
                    x => x.IdDocumento == idSeleccionado.Value);
        }
    }


    
    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        CargarFormulario();

        if (query.TryGetValue("id", out var valor) &&
            int.TryParse(valor?.ToString(), out int id) &&
            id > 0)
        {
            var verificacion =
                _servicio.ObtenerVerificacionPorId(id);

            if (verificacion is not null)
            {
                CargarDesde(verificacion);
                return;
            }
        }

        PrepararNuevo();
    }


    
    private void CargarDesde(Verificacion verificacion)
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


    
    private void PrepararNuevo()
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


    
    private void LimpiarErrores()
    {
        DocumentoError = string.Empty;
        FechaVerificacionError = string.Empty;
        ResultadoError = string.Empty;
        ObservacionError = string.Empty;
    }


    
    private bool ValidarFormulario()
    {
        LimpiarErrores();

        if (DocumentoSeleccionado is null ||
            _servicio.ObtenerDocumentoPorId(
                DocumentoSeleccionado.IdDocumento) is null)
        {
            DocumentoError =
                "Debe seleccionar un documento válido.";
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

        if (ModoEditar &&
            _servicio.ObtenerVerificacionPorId(_idEditando) is null)
        {
            await Shell.Current.DisplayAlertAsync(
                "No encontrada",
                "La verificación que intentas editar ya no existe.",
                "Aceptar");

            return;
        }

        Verificacion registro = new()
        {
            IdVerificacion = _idEditando,
            IdDocumento = DocumentoSeleccionado!.IdDocumento,
            FechaVerificacion = FechaVerificacion,
            Resultado = ResultadoSeleccionado!,
            Observacion = Observacion.Trim()
        };

        bool editar = ModoEditar;

        if (editar)
        {
            if (!_servicio.ActualizarVerificacion(registro))
                return;
        }
        else
        {
            _servicio.AgregarVerificacion(registro);
        }

        await Shell.Current.DisplayAlertAsync(
            "Operación completada",
            editar
                ? "Verificación actualizada correctamente."
                : "Verificación registrada correctamente.",
            "Aceptar");

        PrepararNuevo();

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

        if (!_servicio.EliminarVerificacion(_idEditando))
        {
            await Shell.Current.DisplayAlertAsync(
                "No encontrada",
                "La verificación ya no existe.",
                "Aceptar");

            return;
        }

        await Shell.Current.DisplayAlertAsync(
            "Eliminada",
            "Verificación eliminada correctamente.",
            "Aceptar");

        PrepararNuevo();

        await Shell.Current.GoToAsync(
            "//verificaciones/lista-verificaciones");
    }


    
    [RelayCommand]
    private async Task CancelarAsync()
    {
        PrepararNuevo();

        await Shell.Current.GoToAsync(
            "//verificaciones/lista-verificaciones");
    }
}
