﻿using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Models.ApiModels;
using BehtashShirzad.Models.DbModels;
 
using BehtashShirzad.Tools;
using Logger;
using Microsoft.EntityFrameworkCore;
using SharedObjects;
using System.Collections.Frozen;
using System.Reflection;

namespace BehtashShirzad.Model.Context.DAL
{
    public class UserDAL
    {

        public static async Task<User> GetUserLogin(UserLoginDto user,string IpAddress)
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    var userDb =  await cn.Users.Where(_=>_.Username==user.Credential || _.PhoneNumber == user.Credential).FirstOrDefaultAsync();
                    if (userDb!= null)
                    {
                        userDb.lastIp = IpAddress;
                        userDb.lastLoginTime = DateTime.Now.ToString();
                        
                        if (userDb.Password == Infrastructure.CreatePassHash(user.Password))
                        {
                            userDb.isLastLoginSuccessfull = true;
                           
                            if (userDb.isAdmin)
                            {
                                userDb.Role = "admin";
                            }

                            
                            return userDb;
                        }
                        userDb.isLastLoginSuccessfull = false;
                    }
                    return null;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
                    return null;
                }
                finally
                {
                    await cn.SaveChangesAsync();
                }
            }

        }

        public static async Task<User> GetUser(UserLoginDto user)
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    var userDb = await cn.Users.Where(_ => _.Username == user.Credential || _.PhoneNumber == user.Credential).AsNoTracking().FirstOrDefaultAsync();
                    if (userDb != null)
                    {
                        if (userDb.Password == Infrastructure.CreatePassHash(user.Password))
                        {
                            if (userDb.isAdmin)
                            {
                                userDb.Role = "admin";
                            }
                            return userDb;
                        }
                    }
                    return null;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });
                    return null;
                }
            }

        }



        public static async Task<User> GetUserById(int userId)
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    var userDb = await cn.Users.Where(_ => _.Id == userId).FirstOrDefaultAsync();
                    if (userDb != null)
                    {
                        return userDb;
                    }
                    return null;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
                    return null;
                }
            }

        }



        public static async Task<bool> VerifyUser(string phoneNumber)
        {
            using (var cn = new DbCommiter())
            {
                try
                {

                    var User = await  cn.Users.Where(_=> _.PhoneNumber==  phoneNumber).FirstOrDefaultAsync();
                    if (User!= null)
                    {
                        User.isVerified = true;
                        await cn.SaveChangesAsync();
                        return true;
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });

                    return false;
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
        public static async Task<Constants.Status> CreateUser(UserRegistrationDto u)
        {
            try
            {
                var user = new User() {
                    Username = u.Username,
                    Password = Infrastructure.CreatePassHash(u.Password),
                    PhoneNumber=u.PhoneNumber ,
                    Email = u.Email
                };

                if (_IsExist(user)) 
                    return Constants.Status.UserExists; 

                using (var cn = new DbCommiter())
                {
                    await cn.Users.AddAsync(user);
                    await cn.SaveChangesAsync();
                    return Constants.Status.Registered;
                }
            }
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
                return Constants.Status.Fail;
            }
        }

        public static async Task<Constants.Status> UpdateUserNumber(User u)
        {
            try
            {

            using (var cn = new DbCommiter())
            {
               var user = await cn.Users.Where(_ => _.Username == u.Username).FirstOrDefaultAsync();

                if (user is not null)
                {
                    if (!user.isVerified)
                    {

                        user.PhoneNumber = u.PhoneNumber;
                        await cn.SaveChangesAsync();
                        return Constants.Status.Success;
                    }
                    return Constants.Status.UserVerified;
                }
                return Constants.Status.Fail;
            }
            }
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });

                return Constants.Status.Fail;
            }

        }

        static bool _IsExist(User u)
        {
            using (var cn = new DbCommiter())
                return cn.Users.AnyAsync(_ => _.Username == u.Username || _.PhoneNumber == u.PhoneNumber).Result;


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
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
                return false;
            }
        }
    }
}
