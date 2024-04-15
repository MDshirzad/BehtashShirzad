namespace Logger
{
    using Newtonsoft.Json;
    using Services;
    using SharedObjects.DTO;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class Log
    {
        public static void CreateLog( LogDto logDto, [CallerMemberName] string methodName = "")
        {
            logDto.MethodName = methodName;
            using (var rabbit = new RabbitCommunicator()) {

                
                var log = JsonConvert.SerializeObject(logDto);
                rabbit.publishLog(log);
            }
        }
    }
}
