using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducerService.Data;
using ProducerService.Model;
using UseRabbitMQ;

namespace ProducerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RabbitMQPublisher _publisher;

        public ProducerController(ApplicationDbContext dbContext, RabbitMQPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.SendMessages.ToListAsync());
        }

        [HttpPost]
        public IActionResult Add(SendMessage model)
        {
            try
            {
                _dbContext.SendMessages.Add(model);
                _dbContext.SaveChanges();
                _publisher.Publish(new SendMessageInQueue {Name =model.Name });
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception for further investigation
                Console.WriteLine("DbUpdateException occurred: " + ex.Message);

                // Return an appropriate error response
                return StatusCode(500, "An error occurred while saving the entity changes. Please try again later.");
            }
        }


    }
}
