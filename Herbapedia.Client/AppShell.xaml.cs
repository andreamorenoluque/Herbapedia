using Herbapedia.Client.Servicios;

namespace Herbapedia.Client
{
    public partial class AppShell : Shell
    {
        private Auth _auth;
        public AppShell(IServiceProvider services)
        {
            InitializeComponent();
            _auth = services.GetRequiredService<Auth>();
            //Rutas
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("Register", typeof(RegisterPage));
            Routing.RegisterRoute("PlantPage", typeof(PlantPage));
            Routing.RegisterRoute("PerfilPage", typeof(PerfilUsuarioPage));
            Routing.RegisterRoute("MostrarPlanta", typeof(MostrarPlantaPage));
            Routing.RegisterRoute("PlantView", typeof(PlantViewPage));
            Routing.RegisterRoute("ForoPage", typeof(ForoPage));
            Routing.RegisterRoute("NewPostPage", typeof(NewPostPage));
            Routing.RegisterRoute("PostViewPage", typeof(PostViewPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));
            Routing.RegisterRoute("ChatMessage", typeof(ChatMessagePage));

            OnShellNavigated(null, null);
            this.Navigated += OnShellNavigated;
        }

        private void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
        {
            bool isLogged = _auth.Usuario != null;

            if (!isLogged)
            {
                PerfilUsuarioPage.IsVisible = false;
                ChatPage.IsVisible = false;
            }
            else
            {
                PerfilUsuarioPage.IsVisible = true;
                ChatPage.IsVisible = true;
            }
        }
    }
}
