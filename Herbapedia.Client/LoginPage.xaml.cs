using Herbapedia.Client.Servicios;

namespace Herbapedia.Client;

public partial class LoginPage : ContentPage
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    public LoginPage(IServiceProvider services)
	{
		InitializeComponent();
        _auth = services.GetService<Auth>();
        _api = services.GetService<APIClient>();
    }
	public async void OnLoginButtonClicked(object sender, EventArgs e)
	{
        bool logged = await _auth.Login(UserEntry.Text, PasswordEntry.Text);
        if (logged)
        {
            await Shell.Current.GoToAsync("MostrarPlanta");
        }
        else
        {
            DisplayAlert("Error", "El usuario o contraseña no son correctos", "Ok");
        }
    }
}