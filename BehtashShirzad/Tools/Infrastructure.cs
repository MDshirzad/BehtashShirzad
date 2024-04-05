using System.Security.Cryptography;
using System.Text;

namespace ElliotStore.Tools
{
    public static class Infrastructure
    {

        internal static string CreatePassHash(string plain)
        {
            string salt = "_"+plain[0..2] + plain[4..6]+"@";

            using(var hasher = SHA256.Create())
            {
                var byteHash = Encoding.UTF8.GetBytes(plain + salt);
                return Convert.ToBase64String(hasher.ComputeHash(byteHash)).Replace("-","");
            }



        }

    }
}
