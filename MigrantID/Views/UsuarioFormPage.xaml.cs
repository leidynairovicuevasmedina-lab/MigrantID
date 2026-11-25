
using MigrantID.ViewModels;

namespace MigrantID.Views;

public partial class UsuarioFormPage : ContentPage
{
    public UsuarioFormPage()
    {
        InitializeComponent();

        // Conectar la pantalla con su ViewModel.
        BindingContext = new UsuarioFormViewModel();
    }
}
