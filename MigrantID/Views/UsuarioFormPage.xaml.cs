
using MigrantID.ViewModels;

namespace MigrantID.Views;

public partial class UsuarioFormPage : ContentPage, IQueryAttributable
{
    private readonly UsuarioFormViewModel _viewModel;

    public UsuarioFormPage()
    {
        InitializeComponent();

        _viewModel = new UsuarioFormViewModel();

        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _viewModel.ApplyQueryAttributes(query);
    }
}
