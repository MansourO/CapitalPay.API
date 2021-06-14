using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class RabbitMqConfig
    {
        public string Hostname { get; set; }

        public string QueueName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public bool Enabled { get; set; }
    }
}