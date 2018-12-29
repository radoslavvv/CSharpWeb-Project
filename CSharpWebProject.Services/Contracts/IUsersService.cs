using CSharpWebProject.Models;

namespace CSharpWebProject.Services
{
    public interface IUsersService
    {
        User GetUserByUsername(string username);
        string GetUserIdByUsername(string username);

    }
}