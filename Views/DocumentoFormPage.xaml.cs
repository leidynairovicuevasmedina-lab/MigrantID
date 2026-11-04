
using MigrantID.ViewModels;

namespace MigrantID.Views;

public partial class DocumentoFormPage : ContentPage
{
    private readonly DocumentoFormViewModel _viewModel;

    public DocumentoFormPage()
    {
        InitializeComponent();

        _viewModel = new DocumentoFormViewModel();

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.CargarDocumentoEnEdicion();
    }
}
