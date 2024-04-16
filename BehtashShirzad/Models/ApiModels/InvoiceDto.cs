using BehtashShirzad.Models.Base;
using Newtonsoft.Json;

namespace BehtashShirzad.Model.ApiModels
{
    public class InvoiceDto:IBaseDto
    {
        
        public int[] products { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
