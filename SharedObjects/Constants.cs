using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects
{
    public static class Constants
    {

     public     enum Status
        {
            SENT,
            NotSent,
            NotCorrect,
            Correct,
            Success,
            Fail,
            UserExists,
            Registered,
            UserVerified


        }


        public   enum ApiType
        {
            SMS_OTP,
            SMS_FORGOTPASS,
            TELEGRAM,
            EMAIL

        }

    }
}
