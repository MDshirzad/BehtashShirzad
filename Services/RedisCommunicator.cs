using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
namespace Services
{
    public class RedisCommunicator:IDisposable
    {


        ConnectionMultiplexer redis;
        public RedisCommunicator() {

              redis = ConnectionMultiplexer.Connect("localhost");
            

        }
        public async Task<bool> AddOtpValueAsync(string phoneNumber, string otpCode)
        {
            try
            {


                IDatabase db = redis.GetDatabase();
                var ttl = TimeSpan.FromMinutes(2);
             await   db.StringSetAsync(phoneNumber, otpCode);
             await   db.KeyExpireAsync(phoneNumber, ttl);
                
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> AddValueAsync(string key,string value)
        {
            try
            {

            
            IDatabase db = redis.GetDatabase();
           await db.StringSetAsync(key, value);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<string> GetValueAsync(string key )
        {
            try
            {  
                IDatabase db = redis.GetDatabase();
                var res =  await db.StringGetAsync(key);
                
                return res;
              
            }
            catch (Exception)
            {

                return "";
            }
        }

        public void Dispose()
        {
            redis.Close();

            redis.Dispose();
        }

        

    }
}
