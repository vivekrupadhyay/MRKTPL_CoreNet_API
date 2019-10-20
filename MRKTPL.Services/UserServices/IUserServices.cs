
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System.Collections.Generic;

namespace MRKTPL.Services.UserServices
{
    public interface IUserServices
    {
        bool InsertUsers(UserMaster user);
        bool CheckEmailExits(string email);
        UserMaster GetUsersbyId(int userid);
        bool DeleteUsers(int userid);
        bool UpdateUsers(UserMaster role);
        List<UserMaster> GetAllUsers();
        bool AuthenticateUsers(string email, string password);
        LoginResponse GetUserDetailsbyCredentials(string email);
    }
}