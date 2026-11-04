
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
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarUsuarios();
    }

    private void CargarUsuarios()
    {
        var usuarios = _servicio.Usuarios;

        UsuariosCollection.ItemsSource = null;
        UsuariosCollection.ItemsSource = usuarios;
    }

    private async void OnAgregarClicked(
        object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//usuario-form");
    }

    private async void OnEditarClicked(
        object sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Usuario usuario)
        {
            return;
        }

        // Se conserva el comportamiento original.
        await DisplayAlertAsync(
            "Editar usuario",
            $"Usuario seleccionado: {usuario.Nombre}",
            "Aceptar");
    }

    private async void OnEliminarClicked(
        object sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Usuario usuario)
        {
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Eliminar usuario",
            $"¿Deseas eliminar a {usuario.Nombre}?",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
            return;

        _servicio.Usuarios.Remove(usuario);

        CargarUsuarios();

        await DisplayAlertAsync(
            "Usuario eliminado",
            "El usuario se ha eliminado correctamente.",
            "Aceptar");
    }
}
