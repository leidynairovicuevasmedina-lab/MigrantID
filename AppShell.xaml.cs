
namespace MigrantID
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            
            FlyoutBehavior = FlyoutBehavior.Disabled;

            
            CurrentItem = Items[0];
        }

        private async void OnLogoutClicked(
            object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlertAsync(
                "Cerrar sesión",
                "¿Deseas cerrar tu sesión?",
                "Sí",
                "No");

            if (!confirmar)
                return;

            
            FlyoutIsPresented = false;
            FlyoutBehavior = FlyoutBehavior.Disabled;

            
            await GoToAsync("//login");
        }
    }
}
