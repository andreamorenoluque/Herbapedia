using Herbapedia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Client.Servicios
{
    public class Auth
    {
        static UserModel _loggedUser;
        private readonly APIClient _api;
        public UserModel Usuario {
            get 
            { 
                return _loggedUser;
            }
        }


        public Auth(IServiceProvider services)
        {
            _api = services.GetService<APIClient>();
        }
        public async Task<bool> Login(string username, string password)
        {
            try
            {
                _loggedUser = await _api.GetObject<UserModel>($"User/Login?loginName={username}&password={password}");
                if (_loggedUser == null || _loggedUser == default(UserModel)) { return false; }
                else { return true; }
            }
            catch(Exception ex)



            {
                return false;
            }
        }

        public void Logout()
        {
            _loggedUser = null;
        }
        public void Authorize(ContentPage NewPage, ContentPage ActualPage)
        {
            _loggedUser = new UserModel();
            _loggedUser.UserRole = new RoleModel();
            switch (NewPage)
            {

                case LoginPage login:
                    if(_loggedUser.UserRole.LoginPage == 1)
                    {
                        ActualPage.Navigation.PushAsync(NewPage);
                    }
                    else
                    {
                        ActualPage.DisplayAlert("Aviso", "No se cuenta con los permisos necesarios para acceder a esta pagina", "Entendido");
                    }
                        break;
                case PlantPage plant:
                    if(_loggedUser.UserRole.PlantEditionPage == 1)
                    {
                        ActualPage.Navigation.PushAsync(NewPage);
                    }
                    else
                    {
                        ActualPage.DisplayAlert("Aviso", "No se cuenta con los permisos necesarios para acceder a esta pagina", "Entendido");
                    }
                    break;

            }
        }
    }
}
