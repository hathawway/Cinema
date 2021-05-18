using Cinema.Domain.Models.Users;
using Cinema.Service.Interfaces;

namespace Cinema.Service
{
    public class OwnSignInManager : ISignIn
    {
        public User user { get; set; }

        public bool SignIn(User user)
        {
            this.user = user;
            return true;
        }
        public bool IsLogged()
        {
            return user != null;
        }
        public void SignOut() 
        {
            user = null;
        }
        public bool IsAdmin() 
        {
            return user.RoleId == 2;
        }
        public bool IsOperator()
        {
            return user.RoleId == 3;
        }
    }
}
