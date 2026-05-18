
using Herbapedia.Client.Servicios;
using Herbapedia.Model;

namespace Herbapedia.Client;

public partial class PerfilUsuarioPage : ContentPage
{
    private readonly APIClient _api;
    private readonly Auth _auth;
    public PerfilUsuarioPage(APIClient api, Auth auth)
    {
        InitializeComponent();
        _api = api; 
        _auth = auth;
        entry_Username.Text = _auth.Usuario.UserName;
        entry_Usersurname.Text = _auth.Usuario.UserSurname;
        entry_UserLoginName.Text = _auth.Usuario.UserLoginName;
        entry_UserPhone.Text = _auth.Usuario.UserPhone;
        entry_UserPassword.Text = _auth.Usuario.UserPassword;
        entry_UserEmail.Text = _auth.Usuario.UserEmail;
        dp_Userbirthdate.Date = (DateTime)_auth.Usuario.UserBirthdate;

        this.Title = _auth.Usuario.UserName;



    }



    public async void OnSaveClicked(object sender, EventArgs e)
    {
        UserModel user = new UserModel();
        user.UserId = _auth.Usuario.UserId;
        if (user != null)
        {

            UserModel usuarioActualizado = new UserModel
            {
                UserId = _auth.Usuario.UserId,
                UserName = entry_Username.Text,
                UserSurname = entry_Usersurname.Text,
                UserBirthdate = dp_Userbirthdate.Date,
                UserLoginName = entry_UserLoginName.Text,
                UserPhone = entry_UserPhone.Text,
                UserEmail = entry_UserEmail.Text,
                UserPassword = entry_UserPassword.Text,
                UserModificationDate = DateTime.Now.ToUniversalTime()
            };

            UserModel? userModificado = await _api.PutObject<UserModel>($"User/{usuarioActualizado.UserId}", usuarioActualizado);


            if (userModificado != null)
            {
                await DisplayAlert("Perfil Actualizado", "Se han guardado los cambios correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Hubo un problema al actualizar el perfil.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "id.", "OK");
        }
    }
    }



