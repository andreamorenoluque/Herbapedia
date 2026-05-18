
using Herbapedia.Client.Servicios;
using System.Diagnostics;

namespace Herbapedia.Client
{
    public partial class MainPage : ContentPage
    {
        private IServiceProvider _services;
        private readonly APIClient _api;
        private readonly Auth _auth;
        public MainPage(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
            _auth = services.GetService<Auth>();
            _api = services.GetService<APIClient>();
        }

        public async void OnRegisterClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("Register");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
        
            try
            {
                await Shell.Current.GoToAsync("Login");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                 DisplayAlert("Error", $"Error: {ex.Message}", "Entendido");
            }
        }
        public async void OnDudasClicked(object sender, EventArgs e)
        {
            //Logica por implementar en vias futuras
        }
        public async void OnGuestClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("MostrarPlanta");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}

