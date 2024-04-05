using ApiCommunicator.DTO.Mellipayamak;
using Newtonsoft.Json;
using SharedObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using static SharedObjects.Constants;
namespace ApiCommunicator
{


    public  sealed  class Provider
    {
        private Provider() { }
        private ApiType _apiType;

        private static Provider _instance;

        public static Provider GetInstance(ApiType apiType)
        {
            if (_instance == null)
            {
                _instance = new Provider();
                _instance._apiType =apiType;
            }
            return _instance;
        }

 

       
       
        public async Task<Status> Call(object ?data) {

            Status res = Status.NotSent;
           


            switch (_apiType)
            {
                case ApiType.SMS_OTP:
                    res = await Dispatcher.SendSMS_OTP((OtpDto)data);
                    break;
                case ApiType.TELEGRAM:

                    res = Status.Correct;
                    break;
                case ApiType.EMAIL:
                    res = Status.Correct;
                    break;
                default:
                    break;
            }
            return res;
             
        }

      

    }
}
