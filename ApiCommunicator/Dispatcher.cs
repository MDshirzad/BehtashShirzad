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
             
                
                using (var client = new HttpClient())
                {
                    var res = await client.PostAsync(Urls.MelliPayamakOtp+ApiKeys.MelliPayamak, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")) ;
                    if (res.IsSuccessStatusCode) {
                        var result  = await res.Content.ReadAsStringAsync();
                         
                      
                        var parseJson = JsonConvert.DeserializeObject<OtpResponse>(result);
                        if (parseJson.status.Equals("ارسال نشده"))   
                            return Status.NotSent;

                        if (parseJson.status.Equals("ارسال موفق بود") && parseJson.code is not null ) {
                            
                            using (var redis = new RedisCommunicator())
                            {
                                var ss = await redis.AddOtpValueAsync(data.To, parseJson.code);
 
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
