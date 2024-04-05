
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCommunicator.DTO.Response
{
    internal class OtpResponse
    {
        [JsonProperty("code")]
        internal string code {  get; set; }
        [JsonProperty("status")]
        internal string status { get; set; }
    }
}
