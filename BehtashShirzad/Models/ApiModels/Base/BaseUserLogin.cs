using BehtashShirzad.Models.Base;
using Newtonsoft.Json;

namespace BehtashShirzad.Models.ApiModels.Base
{
    public class BaseUserLogin:IBaseDto
    {
        public string? UserName { get; set; }
        public  string? Password { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
