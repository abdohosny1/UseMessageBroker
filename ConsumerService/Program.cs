using ConsumerService.BackgroundServices;
using ConsumerService.Data;
using ConsumerService.Extension;
using ConsumerService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UseKafka;
using UseRabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


//use kafka
// Kafka Publisher and Consumer Configuration
var kafkaBootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
var topic = builder.Configuration["Kafka:Topic"];
var groupId = builder.Configuration["Kafka:GroupId"];

builder.Services.AddSingleton<KafkaConsumer>(sp => new KafkaConsumer(kafkaBootstrapServers, topic, groupId));
//use rabbit mq
builder.Services.AddSingleton<RabbitMQConsumer>(sp =>
    new RabbitMQConsumer("localhost", "apiQueue"));

//Register the background service
builder.Services.AddHostedService<RabbitMQBackgroundService>();

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
