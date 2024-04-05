using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using static SharedObjects.Constants;
using SharedObjects.DTO;
using Newtonsoft.Json;
using System.Text;
using ApiCommunicator.DTO.Mellipayamak;

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
                    var res = await client.PostAsync(Urls.MelliPayamakOtp, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")) ;
                    if (res.IsSuccessStatusCode) {
                        var result  = await res.Content.ReadAsStringAsync();

                        if (result.Contains("ارسال نشده"))   
                            return Status.NotSent;
                        if (result.Contains("ارسال شده"))
                            return Status.SENT;
                        


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
