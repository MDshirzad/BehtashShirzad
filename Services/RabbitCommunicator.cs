using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using NetTopologySuite.Geometries;
using System.Globalization;


namespace Services
{
    public sealed class RabbitCommunicator:IDisposable
    {
        private RabbitCommunicator() { }
      

        private static RabbitCommunicator _instance;

        public static RabbitCommunicator GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RabbitCommunicator();
                 
            }
            return _instance;
        }

        IConnection _connection;
        IModel _channel;
        public bool connect()
        {
            try
            {

            
            var factory = new ConnectionFactory { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
                return true;
            }
            catch (Exception ex)
            {

               return false;
            }

        }

        public bool createQueue(string _queue) {
            try
            {
                _channel.QueueDeclare(queue: _queue,
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

            
                return true;
            }catch (Exception ex) { return false; }
        }

        public bool createExchange(string _exchange)
        {
            try
            {
            _channel.ExchangeDeclare(_exchange, "direct", false, false);
        return true;
            }
            catch (Exception)
            {

               return false ;
            }

        }

        public bool bindQueueToExchange( string queue, string exchange,string routkey) {
            try
            {
                _channel.QueueBind(queue, exchange, routkey);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            
        }

        public bool publishMessage(string message,string routKey,string Exchange)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: Exchange,
                         routingKey: routKey,
                         basicProperties: null,
                         body: body);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
          

        }

        
        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }



        public bool publishLog(string message)
        {

            var RoutKey = "Log";
            var Exchange = "LogExchange";
            var Queue = "LogQueue";
            if (this.connect())
                if (this.createExchange(Exchange))
                    if (this.createQueue(Queue))
                        if (this.bindQueueToExchange(Queue, Exchange, RoutKey))
                        {
                            try
                            {
                                var body = Encoding.UTF8.GetBytes(message);
                                _channel.BasicPublish(exchange: Exchange,
                                         routingKey: RoutKey,
                                         basicProperties: null,
                                         body: body);
                                return true;
                            }
                            catch (Exception ex)
                            {

                                return false;
                            }
                          
                        }


            return false;
            

        }
    }
}
