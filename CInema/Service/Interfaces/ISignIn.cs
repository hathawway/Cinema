using Cinema.Domain.Models.Users;

namespace Cinema.Service.Interfaces
{
    public interface ISignIn
    {
        public bool SignIn(User user);

        public bool IsLogged();
    }
}
