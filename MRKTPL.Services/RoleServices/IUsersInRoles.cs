using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Services.RoleServices
{
    public interface IUsersInRoles
    {
        bool AssignRole(RoleRightsMaster usersInRoles);
        bool CheckRoleExists(RoleRightsMaster usersInRoles);
        bool RemoveRole(RoleRightsMaster usersInRoles);
        List<AssignRolesViewModel> GetAssignRoles();
    }
}
