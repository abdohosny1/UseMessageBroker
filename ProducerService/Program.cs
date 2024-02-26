using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProducerService.Data;
using ProducerService.Extension;
using ProducerService.Interfaces;
using UseKafka;
using UseRabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var kafkaBootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
var topic = builder.Configuration["Kafka:Topic"];

// Registering KafkaPublisher with a factory delegate
builder.Services.AddSingleton<KafkaPublisher>( sp=>new KafkaPublisher( kafkaBootstrapServers, topic));

builder.Services.AddSingleton<RabbitMQPublisher>(sp => new RabbitMQPublisher("localhost", queueName: "apiQueue"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
