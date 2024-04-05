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


        public bool AddValue(string key,string value)
        {
            try
            {

            
            IDatabase db = redis.GetDatabase();
            db.StringSet(key, value);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string GetValue(string key )
        {
            try
            {  
                IDatabase db = redis.GetDatabase();
                return db.StringGet(key);
              
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
