
using MigrantID.ViewModels;

namespace MigrantID.Views;

public partial class SolicitudFormPage : ContentPage
{
    private readonly SolicitudFormViewModel _viewModel;

    public SolicitudFormPage()
    {
        InitializeComponent();

        _viewModel = new SolicitudFormViewModel();

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.CargarFormulario();
    }
}
