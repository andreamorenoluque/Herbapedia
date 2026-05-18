using Herbapedia.Client.Servicios;
using Herbapedia.Model;

namespace Herbapedia.Client;

public partial class RegisterPage : ContentPage
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    public RegisterPage(APIClient api, Auth auth)
    {
        InitializeComponent();
        _api = api;
        _auth = auth;
    }
    public async void OnRegisterButtonClicked(object sender, EventArgs e)
    {

        UserModel newUser = new UserModel()
        {
            UserName = NameEntry.Text,
            UserSurname = SurnameEntry.Text,
            UserPassword = PasswordEntry.Text,
            RoleID = Convert.ToInt32(TypeEntry.Text),
            UserLoginName = $"{NameEntry.Text.ToLower()}.{SurnameEntry.Text.ToLower()}",
            UserEmail = EmailEntry.Text,
            UserPhone = PhoneEntry.Text,
            UserCreationDate = DateTime.Now.ToUniversalTime(),
            UserBirthdate = DateTime.Now.ToUniversalTime()
        };
        await _api.PostObject<UserModel>("User", newUser);
        await Shell.Current.GoToAsync("Login");

    }
}