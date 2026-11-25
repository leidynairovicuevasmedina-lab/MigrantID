
using MigrantID.Models;
using MigrantID.Services;

namespace MigrantID.Views;

public partial class UsuariosPage : ContentPage
{
    private readonly MigrantIDService _servicio =
        MigrantIDService.Instancia;

    
    public UsuariosPage()
    {
        InitializeComponent();

        UsuariosCollection.ItemsSource = _servicio.Usuarios;
    }

    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        CargarUsuarios();
    }

    private void CargarUsuarios()
    {
        UsuariosCollection.ItemsSource = _servicio.Usuarios;
    }

    
    private async void OnAgregarClicked(
        object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//usuario-form?id=0");
    }

    
    private async void OnEditarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Usuario usuario)
        {
            return;
        }

        
        await Shell.Current.GoToAsync(
            $"//usuario-form?id={usuario.IdUsuario}");
    }

    
    private async void OnEliminarClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Usuario usuario)
        {
            return;
        }

        
        bool confirmar = await DisplayAlertAsync(
            "Eliminar usuario",
            $"¿Estás seguro de eliminar a {usuario.Nombre}?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

      
        bool eliminado = _servicio.EliminarUsuario(
            usuario.IdUsuario);

        if (eliminado)
        {
            await DisplayAlertAsync(
                "Usuario eliminado",
                "El usuario fue eliminado correctamente.",
                "Aceptar");

            CargarUsuarios();
        }
        else
        {
            await DisplayAlertAsync(
                "Error",
                "No se encontró el usuario seleccionado.",
                "Aceptar");
        }
    }
}
