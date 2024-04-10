using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using static SharedObjects.Constants;
using SharedObjects.DTO;
using Newtonsoft.Json;
using System.Text;
using ApiCommunicator.DTO.Mellipayamak;
using Services;
using ApiCommunicator.DTO.Response;

namespace ApiCommunicator
{
    internal   class Dispatcher
    {

   

      static  internal async Task<Status> SendSMS_OTP(OtpDto data)
        {
            try
            {
                Random rnd = new Random();

                var number = rnd.Next(10000,30000).ToString();
                var Message =JsonConvert.SerializeObject( new { bodyId=  208577 , to=data.To, args=new List<string> { number.ToString() } });
                using (var client = new HttpClient())
                {
                    var res = await client.PostAsync(Urls.MelliPayamakOtp+ApiKeys.MelliPayamak, new StringContent(Message, Encoding.UTF8, "application/json")) ;
                    if (res.IsSuccessStatusCode) {
                        var result  = await res.Content.ReadAsStringAsync();
                         
                      
                        var parseJson = JsonConvert.DeserializeObject<OtpResponse>(result);
                        if (parseJson.status.Equals("ارسال نشده"))   
                            return Status.NotSent;

                        if (parseJson.status.Equals("ارسال موفق بود")  ) {
                            
                            using (var redis = new RedisCommunicator())
                            {
                                var ss = await redis.AddOtpValueAsync(data.To, number);
 
                            }


                            return Status.SENT;
                        }


                    }
                   
                    return Status.NotSent;
                }
               


            }
            catch (Exception ex)
            {

                return Status.Fail;
            }
            

        }






    }
}
