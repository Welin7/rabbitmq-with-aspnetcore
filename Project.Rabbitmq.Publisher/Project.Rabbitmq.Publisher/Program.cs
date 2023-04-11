using RabbitMQ.Client;
using System.Text;

//We define a connection to the Rabbitmql node on local host
var fatctory = new ConnectionFactory()
{
    Uri = new Uri(@"amqp://guest:guest@127.0.0.1:5672/"),
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
    AutomaticRecoveryEnabled = true
};

//We open connection with a Rabbitmq node
using (var connection = fatctory.CreateConnection())

//We create a channel where we will define a queue, a message and publish the message
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "Queue one", //Name of queue
                         durable: false, //If equal to true, the queue remains active after the server is restarted
                         exclusive: false, //If equal to true, it can only be accessed via the current connection and are deleted when closing the connection
                         autoDelete: false, //If equal to true, will be automatically deleted after consumers use the queue
                         arguments: null);

    //Create message
    string message = "Generate message using RabbitMQ"; 

    //Create message to queue and we encode the message as a byte array
    var body = Encoding.UTF8.GetBytes(message); 

    //We publish the message informing the queue and the body of the message
    channel.BasicPublish(exchange: "",
                         routingKey: "Queue one",
                         basicProperties: null,
                         body: body);

    //Print the message published
    Console.WriteLine($" [X] Send: {message}");

    Console.ReadLine();
}

