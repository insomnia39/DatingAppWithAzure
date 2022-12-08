using DatingApp.DAL.Model;

namespace DatingApp.BLL.JWT
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
