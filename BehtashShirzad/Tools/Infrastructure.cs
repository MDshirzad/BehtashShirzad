using Azure;
using BehtashShirzad.Model;
using Microsoft.IdentityModel.Tokens;
using SharedObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BehtashShirzad.Tools
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
        {
            if (string.IsNullOrEmpty(phone))
                return Constants.Status.NotCorrect;


            return (phone.Trim().Length == 11) ? Constants.Status.Correct : Constants.Status.NotCorrect; 
         
        }


         
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
            if (string.IsNullOrEmpty(phone))
                return Constants.Status.NotCorrect;
          
                var IranianPatternNumber = "09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}";

            Regex r = new Regex(IranianPatternNumber);
                
            return r.IsMatch(phone.Trim())?  Constants.Status.Correct : Constants.Status.NotCorrect;
        }


        internal static Constants.Status IsPasswordLengthValid(string pass)
            
        {
            if ( string.IsNullOrEmpty(pass))
             return Constants.Status.NotCorrect;
             
            if (pass.Trim().Length < 6 || pass.Trim().Length > 12)
            {
                return Constants.Status.NotCorrect;
            
            
            }
            return Constants.Status.Correct;
        }

        internal static string GenerateToken(List<Claim> claims)
        {

            try
            {
                
                
                 
                    
                    var signingKey = new SymmetricSecurityKey(SecreteKeyJWT);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
                        
                    
                   
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    return tokenHandler.WriteToken(token);
                 
            }

            catch (Exception ex)
            {

                return "";
            }
        }

         
        internal static IEnumerable<Claim> GetClaims(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            return token.Claims;
        }


        internal static string EncodeForSafety(string plain)
        {

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
        }
    }
    }
