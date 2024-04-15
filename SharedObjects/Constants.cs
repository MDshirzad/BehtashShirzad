using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects
{
    public static class Constants
    {

     public   enum Status
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

        public enum LogType
        {
            Success,
            Fail,
            Warning,
            Critical,
            Info,
            Error

        }


        public   enum ApiType
        {
            SMS_OTP,
            SMS_FORGOTPASS,
            TELEGRAM,
            EMAIL

        }

        public static string LogonRequestText = "New Login Request";
        public static string RegisterRequestText = "New Registration Request";
        public static string OkResponse = "Response Ok";
    }
}
