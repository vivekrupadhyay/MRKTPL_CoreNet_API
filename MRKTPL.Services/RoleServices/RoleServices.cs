using Microsoft.Extensions.Configuration;
using MRKTPL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MRKTPL.Services.RoleServices
{
    public class RoleServices : IRoleServices
    {
        #region Field
        private readonly MarketPlaceCoreContext context;
        private readonly IConfiguration configuration;
        #endregion
        #region CTor
        public RoleServices(MarketPlaceCoreContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }
        #endregion
        #region Methods
        public bool CheckRoleExits(string roleName)
        {
            try
            {
                int query = (from c in context.Roles
                             where c.RoleName == roleName
                             select c).Count();
                return query > 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void InsertRole(RoleMaster role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
        }
        public RoleMaster GetRolebyId(int roleId)
        {
            RoleMaster role = new RoleMaster();
            try
            {
                string connectionString = configuration.GetConnectionString("DatabaseConnStr");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("USP_MRKTPL_GETALLROLEBYID", con)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@RoleID", roleId);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (sdr.Read())
                    {
                        role.RoleID = Convert.ToInt32(sdr["RoleID"]);
                        role.RoleName = Convert.ToString(sdr["RoleName"]);
                        role.IsActive = Convert.ToBoolean(sdr["IsActive"]);
                        role.CreatedBy = Convert.ToInt64(sdr["CreatedBy"]);
                        role.CreatedDate = Convert.ToDateTime(sdr["CreatedDate"]);
                        role.ModifiedBy = Convert.ToInt64(sdr["ModifiedBy"]);
                    }

                    con.Close();
                }
                return role;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public bool UpdateRole(RoleMaster role)
        {
            try
            {
                context.Entry(role).Property(x => x.RoleName).IsModified = true;
                context.Entry(role).Property(x => x.RoleDescription).IsModified = true;
                context.Entry(role).Property(x => x.IsActive).IsModified = true;
                //context.Entry(role).Property(x => x.Status).IsModified = true;

                int result = context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<RoleMaster> GetAllRole()
        {
            try
            {

                var query = (from c in context.Roles select c).ToList();
                return query;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteRole(int roleId)
        {
            int i = 0;
            try
            {
                RoleMaster role = context.Roles.Find(roleId);
                if (role != null)
                {
                    context.Roles.Remove(role);
                    i = context.SaveChanges();
                }
                return i > 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    #endregion
}

