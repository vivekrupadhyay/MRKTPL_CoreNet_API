
using Microsoft.EntityFrameworkCore;
using MRKTPL.Data.Entities;
using MRKTPL.Data.Helper;
using MRKTPL.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRKTPL.Services.UserServices
{
    public class UserServices : IUserServices
    {
        #region Field
        private readonly MarketPlaceCoreContext context;
        internal DbSet<UserMaster> DbSet;
        #endregion
        #region CTor
        public UserServices(MarketPlaceCoreContext _context)
        {
            context = _context;
            DbSet = context.Set<UserMaster>();
        }
        #endregion
        #region Methods
        public bool InsertUsers(UserMaster user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }


                DbSet.Add(user);
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
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public bool CheckEmailExits(string email)
        {
            try
            {
                //IQueryable<Users> query = DbSet;
                int result = (from c in context.Users
                              where c.Email == email
                              select c).Count();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public UserMaster GetUsersbyId(int userid)
        {
            //return DbSet.Find(userid);
            UserMaster result = (from user in context.Users
                            where user.UserID == userid
                            select user).FirstOrDefault();

            return result;
        }
        public bool UpdateUsers(UserMaster user)
        {
            context.Entry(user).Property(x => x.Email).IsModified = true;
            context.Entry(user).Property(x => x.MobileNo).IsModified = true;
            context.Entry(user).Property(x => x.IsActive).IsModified = true;
            context.Entry(user).Property(x => x.FirstName).IsModified = true;
            context.Entry(user).Property(x => x.LastName).IsModified = true;
            context.Entry(user).Property(x => x.Password).IsModified = true;
            context.Entry(user).Property(x => x.Gender).IsModified = true;

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
        public bool AuthenticateUsers(string email, string password)
        {
            int result = (from c in context.Users
                          where c.Email == email
                          && c.Password == password
                          select c).Count();
            return result > 0 ? true : false;
        }
        public LoginResponse GetUserDetailsbyCredentials(string email)
        {
            try
            {
                LoginResponse result = (from user in context.Users
                                        
                                        join userinrole in context.RoleRights on user.UserID equals userinrole.UserID
                                        where user.Email == email

                                        select new LoginResponse
                                        {
                                            UserId = user.UserID,
                                            RoleId = userinrole.RoleID,
                                            Status = user.IsActive,
                                            Email = user.Email
                                        }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public System.Collections.Generic.List<UserMaster> GetAllUsers()
        {
            System.Collections.Generic.List<UserMaster> result = (from user in context.Users
                                                             where user.IsActive == true
                                                             select user).ToList();

            return result;
        }
        public bool DeleteUsers(int userId)
        {
            UserMaster removeuser = (from user in context.Users
                                where user.UserID == userId
                                select user).FirstOrDefault();
            if (removeuser != null)
            {
                context.Users.Remove(removeuser);
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
            else
            {
                return false;
            }
        }
        #endregion
    }
}







