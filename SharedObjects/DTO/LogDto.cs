using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.Threading.Tasks;
using SharedObjects.DTO;
namespace SharedObjects.DTO
{
    public class LogDto
    {
      
         
        public string Type
        {
            get
            {
                return LogType.HasValue ? LogType.Value.ToString() : null;
            }
            set
            {
                if (value != null)
                {
                    LogType = Enum.Parse<Constants.LogType>(value);
                }
                else
                {
                    LogType = null;
                }
            }
        }

        public   string? MethodName { get; set; }
       
        public required string? Description { get; set; }
        public string? Extra { get; set; }
 
        public required Constants.LogType? LogType { get; set; }

        public bool ShouldSerializeLogType()
        {
            // Return false to always ignore 'Secret' during serialization
            return false;
        }

    }
}
