
using MigrantID.ViewModels;

namespace MigrantID.Views;

public partial class UsuarioFormPage : ContentPage
{
    public UsuarioFormPage()
    {
        InitializeComponent();

        
        BindingContext = new UsuarioFormViewModel();
    }
}
