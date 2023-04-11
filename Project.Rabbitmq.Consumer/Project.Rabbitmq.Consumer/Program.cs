using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

    //Requests delivery of messages asynchronously and provides a callback
    var consumer = new EventingBasicConsumer(channel);

    //Receives the message from the queue, converts it to a string and prints the message to the console
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [X] Received: {message}");
    };

    //We indicate the consumption of the message
    channel.BasicConsume(queue: "Queue one",
                         autoAck: true,
                         consumer: consumer);

    Console.ReadLine();
}
