using Cinema.Domain.Models.Users;

namespace Cinema.Service.Interfaces
{
    public interface ISignIn
    {
        public bool SignIn(User user);
        public void SignOut();
        public bool IsSignedIn();
        public bool IsAdmin();
        public bool IsOperator();
        public bool IsUser();
    }
}
