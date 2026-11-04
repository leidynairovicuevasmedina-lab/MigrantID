
using MigrantID.ViewModels;

namespace MigrantID.Views;

public partial class VerificacionFormPage : ContentPage
{
    private readonly VerificacionFormViewModel _viewModel;

    public VerificacionFormPage()
    {
        InitializeComponent();

        _viewModel = new VerificacionFormViewModel();

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.CargarFormulario();
    }
}
