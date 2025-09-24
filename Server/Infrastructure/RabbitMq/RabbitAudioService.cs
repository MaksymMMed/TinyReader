using Application.Common;
using Infrastructure.RabbitMq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitAudioService : BackgroundService, IRabbitAudioService
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _consumerChannel;

    public RabbitAudioService(ConnectionFactory factory)
    {
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = await _factory.CreateConnectionAsync();
        _consumerChannel = await _connection.CreateChannelAsync();

        await _consumerChannel.QueueDeclareAsync(
            queue: "audios",
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        Console.WriteLine("RabbitMQ is online");
    }

    public async Task<Result<string>> SendAudioAsync(IFormFile audioFile)
    {
        if (audioFile == null || audioFile.Length == 0)
        {
            return Result<string>.Fail("No audio file provided.")!;
        }
        using var memoryStream = new MemoryStream();
        await audioFile.CopyToAsync(memoryStream);
        var audioData = memoryStream.ToArray();
        var contentType = audioFile.ContentType;

        var channel = await _connection!.CreateChannelAsync();

        var replyQueue = await channel.QueueDeclareAsync("", exclusive: true);
        var consumer = new AsyncEventingBasicConsumer(channel);

        var tcs = new TaskCompletionSource<string>();
        string correlationId = Guid.NewGuid().ToString();

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                string response = Encoding.UTF8.GetString(ea.Body.ToArray());
                tcs.TrySetResult(response);
            }
            await Task.Yield();
        };

        await channel.BasicConsumeAsync(
            queue: replyQueue.QueueName,
            autoAck: true,
            consumer: consumer
        );

        var props = new BasicProperties
        {
            ContentType = contentType,
            CorrelationId = correlationId,
            ReplyTo = replyQueue.QueueName
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "audios",
            mandatory: false,
            basicProperties: props,
            body: audioData
        );

        var result = await tcs.Task;
        await channel.CloseAsync();
        return Result<string>.Success(result)!;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_consumerChannel != null)
            await _consumerChannel.CloseAsync();

        if (_connection != null)
            await _connection.CloseAsync();

        await base.StopAsync(cancellationToken);
    }
}
