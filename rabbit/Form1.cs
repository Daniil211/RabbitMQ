using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;
using System.Threading;

namespace rabbit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var counter = 0;
            do
            {
                int timeToSleep = new Random().Next(1000, 3000);
                Thread.Sleep(timeToSleep);
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var chanel = connection.CreateModel())
                {
                    chanel.QueueDeclare(queue: "dex-queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message = "Hello Consumer N" + counter++;

                    var body = Encoding.UTF8.GetBytes(message);
                    chanel.BasicPublish(exchange: "",
                        routingKey: "dex-queue",
                        basicProperties: null,
                        body: body);

                    textBox1.Text += "Message is sent into Default Exchange [N" + counter++ + "]" + Environment.NewLine;
                }
            }
            while (true);
        }
    }
}
