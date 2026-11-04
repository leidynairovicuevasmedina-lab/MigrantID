
using System;
using Microsoft.Maui.Controls;

namespace MigrantID.Views
{
    public partial class LoginPage : ContentPage
    {
        private const string CorreoCorrecto =
            "pruebamigrantidrd@gmail.com";

        private const string ContrasenaCorrecta = "1234";

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Shell.Current.FlyoutBehavior =
                FlyoutBehavior.Disabled;

            CorreoEntry.Text = string.Empty;
            ContrasenaEntry.Text = string.Empty;

            MensajeErrorLabel.IsVisible = false;
        }

        private async void OnLoginClicked(
            object sender, EventArgs e)
        {
            string correo =
                CorreoEntry.Text?.Trim() ?? string.Empty;

            string contrasena =
                ContrasenaEntry.Text ?? string.Empty;

            
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                MensajeErrorLabel.Text =
                    "Completa el correo y la contraseña.";

                MensajeErrorLabel.IsVisible = true;
                return;
            }

            
            if (correo == CorreoCorrecto &&
                contrasena == ContrasenaCorrecta)
            {
                MensajeErrorLabel.IsVisible = false;

                CorreoEntry.Text = string.Empty;
                ContrasenaEntry.Text = string.Empty;

                
                await Shell.Current.GoToAsync(
                    "//app/principal");

                
                Shell.Current.FlyoutBehavior =
                    FlyoutBehavior.Flyout;
            }
            else
            {
                MensajeErrorLabel.Text =
                    "Correo o contraseña incorrectos.";

                MensajeErrorLabel.IsVisible = true;

                ContrasenaEntry.Text = string.Empty;
            }
        }
    }
}
