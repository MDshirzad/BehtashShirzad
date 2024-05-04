using Newtonsoft.Json;

namespace BehtashShirzad.Models.ApiModels
{
    public class ChangeProductVisibilityDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Visible")]
        public string Visible { get; set; }
    }
}
