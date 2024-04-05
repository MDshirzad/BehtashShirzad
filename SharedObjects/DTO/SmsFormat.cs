using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.DTO
{
    public class SmsFormat:object
    {
        public required string FullName  { get; set; }
        public required string Numbers { get; set; }
        public required string Text { get; set; }   
    }
}
