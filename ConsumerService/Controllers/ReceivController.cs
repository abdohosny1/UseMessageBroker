using ConsumerService.Data;
using ConsumerService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsumerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ReceivController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.ReceiveMessages.ToListAsync());
        }

        [HttpPost]
        public IActionResult Add(ReceiveMessage model)
        {
            try
            {
                _dbContext.ReceiveMessages.Add(model);
                _dbContext.SaveChanges();
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
