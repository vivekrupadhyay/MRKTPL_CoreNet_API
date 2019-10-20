using Microsoft.Extensions.Configuration;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Services.RoleServices
{
   public class RoleRightServices:IUsersInRoles
    {
        private readonly MarketPlaceCoreContext context;
        //private readonly IConfiguration _configuration;
        public RoleRightServices(MarketPlaceCoreContext _context)//, IConfiguration configuration)
        {
            context = _context;
          //  _configuration = configuration;
        }
        public bool AssignRole(RoleRightsMaster usersInRoles)
        {
            context.Add(usersInRoles);
            var result = context.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckRoleExists(RoleRightsMaster usersInRoles)
        {
            var result = (from userrole in context.RoleRights
                          where userrole.UserID == usersInRoles.UserID && userrole.RoleID == usersInRoles.RoleID
                          select userrole).Count();

            return result > 0 ? true : false;
        }

        public bool RemoveRole(RoleRightsMaster usersInRoles)
        {
            var role = (from userrole in context.RoleRights
                        where userrole.UserID == usersInRoles.UserID && userrole.RoleID == usersInRoles.RoleID
                        select userrole).FirstOrDefault();
            if (role != null)
            {
                context.RoleRights.Remove(role);
                var result = context.SaveChanges();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<AssignRolesViewModel> GetAssignRoles()
        {
            var result = (from usertb in context.RoleRights
                          join role in context.Roles on usertb.RoleID equals role.RoleID
                          join user in context.Users on usertb.UserID equals user.UserID
                          select new AssignRolesViewModel()
                          {
                              RoleName = role.RoleName,
                              RoleId = usertb.RoleID,
                              UserName = role.RoleName,
                              UserId = usertb.UserID,
                              UserRolesId = usertb.RoleRightsMappingID

                          }).ToList();

            return result;
        }
    }
}
