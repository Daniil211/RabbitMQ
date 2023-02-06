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
using RabbitMQ.Client.Events;

namespace Consumer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var chanel = connection.CreateModel())
            {
                chanel.QueueDeclare(queue: "dex-queue",
                   durable: false,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);
                var consumer = new EventingBasicConsumer(chanel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var mesage = Encoding.UTF8.GetString(body.ToArray());
                    textBox1.Text += "Received message: {0}" + mesage;
                };

                chanel.BasicConsume(queue: "dex-queue",
                    autoAck: true,
                    consumer: consumer);
                textBox1.Text += "Subscribed to the queue 'dex-queue' ";
            }
        }
    }
}
