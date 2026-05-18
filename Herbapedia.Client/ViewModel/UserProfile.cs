using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Client.ViewModel
{

    public class UserProfile : INotifyPropertyChanged
    {
        public UserProfile() { }
        private string _userName;
        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }
        private string _userSurname;
        public string UserSurname
        {
            get => _userSurname;
            set { _userSurname = value; OnPropertyChanged(); }
        }

        private DateTime _userBirthdate;
        public DateTime UserBirthdate
        {
            get => _userBirthdate;
            set { _userBirthdate = value; OnPropertyChanged(); }
        }

        private string _userLoginName;
        public string UserLoginName
        {
            get => _userLoginName;
            set { _userLoginName = value; OnPropertyChanged(); }
        }

        private string _userPhone;
        public string UserPhone
        {
            get => _userPhone;
            set { _userPhone = value; OnPropertyChanged(); }
        }

        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set { _userEmail = value; OnPropertyChanged(); }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
