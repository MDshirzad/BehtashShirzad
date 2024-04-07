using BehtashShirzad.Models.ApiModels;
using ElliotStore.Model.ApiModels;
using ElliotStore.Tools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;

namespace ElliotStore.Model.Context.DAL
{
    public class UserDAL
    {

        public static async Task<User> GetUser(UserLoginDto user)
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    var userDb =  await cn.Users.Where(_=>_.Username==user.UserName).FirstOrDefaultAsync();
                    if (userDb!= null)
                    {
                        if (userDb.Password == Infrastructure.CreatePassHash(user.Password))
                        {
                            return userDb;
                        }
                    }
                    return null;

                }
                catch (Exception)
                {

                    return null;
                }
            }

        }

        public static IEnumerable<User> GetUsers()
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Users.ToFrozenSet();

                }
                catch (Exception)
                {

                    return Enumerable.Empty<User>();
                }
            }

        }
        public static async Task<bool> CreateUser(UserRegistrationDto u)
        {
            try
            {
                var user = new User() { Username = u.Username, Password = Infrastructure.CreatePassHash(u.Password),PhoneNumber=u.PhoneNumber };

                if (_IsExist(user)) { return false; }
                using (var cn = new DbCommiter())
                {
                    await cn.Users.AddAsync(user);
                    await cn.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        static bool _IsExist(User u)
        {
            using (var cn = new DbCommiter())
                return cn.Users.AnyAsync(_ => _.Username == u.Username).Result;


        }

        static public bool DeleteUser(int id)
        {

            try
            {
                using (var db = new DbCommiter())
                {
                    db.Users.Where(_ => _.Id == id).ExecuteDelete();
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
