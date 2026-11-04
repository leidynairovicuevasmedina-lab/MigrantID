
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MigrantID.Models;
using MigrantID.Services;


namespace MigrantID.ViewModels;

public partial class DocumentoFormViewModel : ObservableObject
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

   

    [ObservableProperty]
    private string titulo = "Registrar documento";

    [ObservableProperty]
    private string? tipoDocumento;

    [ObservableProperty]
    private string numeroDocumento = string.Empty;

    [ObservableProperty]
    private string? paisEmision;

    [ObservableProperty]
    private DateTime fechaEmision = DateTime.Today;

    [ObservableProperty]
    private DateTime fechaVencimiento =
        DateTime.Today.AddYears(1);

    [ObservableProperty]
    private string? estado = "En revisión";



    [ObservableProperty]
    private string tipoDocumentoError = string.Empty;

    [ObservableProperty]
    private string numeroDocumentoError = string.Empty;

    [ObservableProperty]
    private string paisEmisionError = string.Empty;

    [ObservableProperty]
    private string fechaEmisionError = string.Empty;

    [ObservableProperty]
    private string fechaVencimientoError = string.Empty;

    [ObservableProperty]
    private string estadoError = string.Empty;

  

    public string[] TiposDocumento { get; } =
    {
        "Pasaporte",
        "Documento de identidad (ID)",
        "Residencia temporal",
        "Residencia permanente",
        "Permiso de permanencia"
    };

    public string[] PaisesEmision { get; } =
    {
        "República Dominicana",
        "Haití",
        "Venezuela",
        "Colombia",
        "Cuba",
        "Estados Unidos",
        "Canadá",
        "México",
        "España",
        "Francia",
        "Brasil",
        "Argentina",
        "Chile",
        "Perú",
        "China",
        "India",
        "Jamaica"
    };

    public string[] EstadosDisponibles { get; } =
    {
        "Vigente",
        "En revisión",
        "Vencido"
    };

    

    partial void OnTipoDocumentoChanged(string? value)
    {
        TipoDocumentoError = string.Empty;
    }

    partial void OnNumeroDocumentoChanged(string value)
    {
        NumeroDocumentoError = string.Empty;
    }

    partial void OnPaisEmisionChanged(string? value)
    {
        PaisEmisionError = string.Empty;
    }

    partial void OnFechaEmisionChanged(DateTime value)
    {
        FechaEmisionError = string.Empty;
        FechaVencimientoError = string.Empty;
    }

    partial void OnFechaVencimientoChanged(DateTime value)
    {
        FechaVencimientoError = string.Empty;
    }

    partial void OnEstadoChanged(string? value)
    {
        EstadoError = string.Empty;
    }

    

    public void CargarDocumentoEnEdicion()
    {
        Documento? documento =
            _servicio.DocumentoEnEdicion;

        if (documento == null)
        {
            LimpiarFormulario();
            return;
        }

        Titulo = "Editar documento";
        TipoDocumento = documento.TipoDocumento;
        NumeroDocumento = documento.NumeroDocumento;
        PaisEmision = documento.PaisEmision;
        FechaEmision = documento.FechaEmision;
        FechaVencimiento = documento.FechaVencimiento;
        Estado = documento.Estado;

        LimpiarErrores();
    }


    private bool ValidarFormulario()
    {
        LimpiarErrores();

        if (string.IsNullOrWhiteSpace(TipoDocumento))
        {
            TipoDocumentoError =
                "Selecciona el tipo de documento.";
        }

        if (string.IsNullOrWhiteSpace(NumeroDocumento))
        {
            NumeroDocumentoError =
                "El número de documento es obligatorio.";
        }

        if (string.IsNullOrWhiteSpace(PaisEmision))
        {
            PaisEmisionError =
                "Selecciona el país de emisión.";
        }

        if (FechaEmision.Date > DateTime.Today)
        {
            FechaEmisionError =
                "La fecha de emisión no puede ser futura.";
        }

        if (FechaVencimiento.Date <= FechaEmision.Date)
        {
            FechaVencimientoError =
                "El vencimiento debe ser posterior a la emisión.";
        }

        if (string.IsNullOrWhiteSpace(Estado))
        {
            EstadoError =
                "Selecciona el estado del documento.";
        }

        return new[]
        {
            TipoDocumentoError,
            NumeroDocumentoError,
            PaisEmisionError,
            FechaEmisionError,
            FechaVencimientoError,
            EstadoError
        }.All(string.IsNullOrEmpty);
    }

   
    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (!ValidarFormulario())
            return;

        Documento? existente =
            _servicio.DocumentoEnEdicion;

        Documento documento = new()
        {
            IdDocumento = existente?.IdDocumento ?? 0,
            TipoDocumento = TipoDocumento!,
            NumeroDocumento = NumeroDocumento.Trim(),
            PaisEmision = PaisEmision!,
            FechaEmision = FechaEmision,
            FechaVencimiento = FechaVencimiento,
            Estado = Estado!
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

        await Shell.Current.DisplayAlertAsync(
            "Operación completada",
            existente == null
                ? "Documento guardado correctamente."
                : "Documento actualizado correctamente.",
            "Aceptar");

        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//documentos/lista-documentos");
    }


    [RelayCommand]
    private async Task CancelarAsync()
    {
        _servicio.DocumentoEnEdicion = null;

        LimpiarFormulario();

        await Shell.Current.GoToAsync(
            "//documentos/lista-documentos");
    }

  
    private void LimpiarErrores()
    {
        TipoDocumentoError = string.Empty;
        NumeroDocumentoError = string.Empty;
        PaisEmisionError = string.Empty;
        FechaEmisionError = string.Empty;
        FechaVencimientoError = string.Empty;
        EstadoError = string.Empty;
    }

    private void LimpiarFormulario()
    {
        Titulo = "Registrar documento";
        TipoDocumento = null;
        NumeroDocumento = string.Empty;
        PaisEmision = null;
        FechaEmision = DateTime.Today;
        FechaVencimiento = DateTime.Today.AddYears(1);
        Estado = "En revisión";

        LimpiarErrores();
    }
}
