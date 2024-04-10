using SharedObjects;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ElliotStore.Tools
{
    public static class Infrastructure
    {

        internal static byte[] SecreteKeyJWT {  get; set; }
        internal static string CreatePassHash(string plain)
        {
            string salt = "_"+plain[0..2] + plain[4..6]+"@";

            using(var hasher = SHA256.Create())
            {
                var byteHash = Encoding.UTF8.GetBytes(plain + salt);
                return Convert.ToBase64String(hasher.ComputeHash(byteHash)).Replace("-","");
            }



        }


        internal static Constants.Status IsPhoneNumberLengthValid(string phone)
           =>   (phone.Trim().Length == 11) ? Constants.Status.Correct  : Constants.Status.NotCorrect;


         
            internal static byte[] GenerateSecretKey(int keySize)
            {
                byte[] keyBytes = new byte[keySize / 8];
                using (var rng = new RNGCryptoServiceProvider())
                {
                     rng.GetBytes(keyBytes);
                }
            SecreteKeyJWT = keyBytes;
            return keyBytes;
            }
         

        internal static Constants.Status IsPhoneNumberFormatValid(string phone)

        {
            var IranianPatternNumber = "09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}";

            Regex r = new Regex(IranianPatternNumber);
                
            return r.IsMatch(phone.Trim())?  Constants.Status.Correct : Constants.Status.NotCorrect;
        }


        internal static Constants.Status IsPasswordLengthValid(string pass)
            
        { 
            if (pass.Trim().Length < 6 || pass.Trim().Length > 12)
            {
                return Constants.Status.NotCorrect;
            }
            return Constants.Status.Correct;
        }

             
  
        }
    }
