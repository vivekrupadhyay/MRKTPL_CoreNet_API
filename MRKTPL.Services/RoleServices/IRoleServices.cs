using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System.Collections.Generic;

namespace MRKTPL.Services.RoleServices
{
    public interface IRoleServices
    {
        void InsertRole(RoleMaster role);
        bool CheckRoleExits(string roleName);
        RoleMaster GetRolebyId(int roleId);
        bool DeleteRole(int roleId);
        bool UpdateRole(RoleMaster role);
        List<RoleMaster> GetAllRole();

        
    }
}
