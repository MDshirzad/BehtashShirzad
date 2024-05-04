using BehtashShirzad.Models.Base;
using Newtonsoft.Json;

namespace BehtashShirzad.Model.ApiModels
{
    public class InvoiceDto:IBaseDto
    {
        [JsonProperty("products")]
        public string[] products { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
