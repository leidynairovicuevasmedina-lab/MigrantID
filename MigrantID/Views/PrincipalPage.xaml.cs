
using System;
using Microsoft.Maui.Controls;

namespace MigrantID.Views
{
    public partial class PrincipalPage : ContentPage
    {
        public PrincipalPage()
        {
            InitializeComponent();
        }

        
        private async void OnDocumentosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//documentos");
        }

        
        private async void OnUsuariosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//usuarios");
        }

        
        private async void OnVerificacionesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//verificaciones");
        }

        
        private async void OnSolicitudesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//solicitudes");
        }
    }
}
