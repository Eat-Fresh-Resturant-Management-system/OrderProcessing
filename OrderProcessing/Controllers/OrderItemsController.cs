using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using OrderProcessing.Data;
using OrderProcessing.Models;

namespace OrderProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly Order_Db _context;
        private readonly ILogger<OrderItemsController> _logger;

        public OrderItemsController(Order_Db context, ILogger<OrderItemsController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
          if (_context.OrderItems == null)
          {
              return NotFound();
          }
            return await _context.OrderItems.ToListAsync();
        }
        [HttpGet("tezxt")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderI2tems()
        {
            return Ok("dasd");
        }
     
        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(string id)
        {
          if (_context.OrderItems == null)
          {
              return NotFound();
          }
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }
        [HttpPost("CreatIe")]
        public async Task<IActionResult> CreateFamily([FromBody] OrderItemDto familyDto)
        {

          
            // Check if the user already has a family
           

            var family = new OrderItem
            {
                Price = familyDto.Price,
                Quantity = familyDto.Quantity,
            };


            _context.OrderItems.Add(family);

            // Set HasFamily property to true for the user

            await _context.SaveChangesAsync();

            return Ok("order created successfully");
        }
        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
          if (_context.OrderItems == null)
          {
              return Problem("Entity set 'Order_Db.OrderItems'  is null.");
          }
            _context.OrderItems.Add(orderItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderItemExists(orderItem.OrderItemId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, orderItem);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrderEmail(int id, OrderItemDto orderItemDto)
        {
            _logger.LogInformation("UpdateUser endpoint is called.");

            var order = await _context.OrderItems.FindAsync(id);
            if (order == null || orderItemDto == null)
            {
                return NotFound();
            }

            if (orderItemDto.Price != null)
            {
                order.Price = orderItemDto.Price;
            }

            if (orderItemDto.Quantity != null)
            {
                order.Quantity = orderItemDto.Quantity;
            }
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated order with orderId {id}");
                await Task.Delay(10000);


                return NoContent();
                
            }
          
            catch (DbUpdateConcurrencyException ex)
            {

                _logger.LogError("Concurrency conflict occurred while updating order: " + ex.Message);

                // Reload the entity from the database to get the latest values
                var entry = ex.Entries.FirstOrDefault();
                if (entry != null)
                {
                    if (order.Price != null) { entry.CurrentValues.SetValues(order.Price); }
                    if (order.Quantity != null) { entry.CurrentValues.SetValues(order.Quantity); }

                    _logger.LogInformation($"Concurrency conflict resolved for user with userId {id}");
                    // Optionally, you can check if the entity has been modified by another user before applying changes
                    // _context.Entry(user).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Concurrency conflict resolved for order with orderid {id}");
                        return NoContent();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        // Handle concurrency conflict
                        _logger.LogError($"Failed to resolve concurrency conflict for order with orderid {id}");
                        return StatusCode(409); // Conflict
                    }
                }
                // Handle other exceptions if needed
                return StatusCode(500); // Internal Server Error
            }
        }
        [HttpPut("update2/{id}")]
        public async Task<IActionResult> UpdateUsereEmail(int id, OrderItemDto orderItemDto)
        {
            _logger.LogInformation("UpdateUser endpoint is called.");

            var user = await _context.OrderItems.FindAsync(id);
            if (user == null || orderItemDto == null)
            {
                return NotFound();
            }

            if (orderItemDto.Price != null)
            {
                user.Price = orderItemDto.Price;
            }

            if (orderItemDto.Quantity != null)
            {
                user.Quantity = orderItemDto.Quantity;
            }

            try
            {
                await Task.Delay(10000);

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated user with userId {id}");

                // Delay for 10 seconds before returning NoContent()

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("Concurrency conflict occurred while updating user: " + ex.Message);

                // Reload the entity from the database to get the latest values
                var entry = ex.Entries.FirstOrDefault();
                if (entry != null)
                {
                    if (user.Price != null) { entry.CurrentValues.SetValues(user.Price); }
                    if (user.Quantity != null) { entry.CurrentValues.SetValues(user.Quantity); }

                    _logger.LogInformation($"Concurrency conflict resolved for user with userId {id}");
                    // Optionally, you can check if the entity has been modified by another user before applying changes
                    // _context.Entry(user).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"Concurrency conflict resolved for user with userId {id}");
                        return NoContent();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        // Handle concurrency conflict
                        _logger.LogError($"Failed to resolve concurrency conflict for user with userId {id}");
                        return StatusCode(409); // Conflict
                    }
                }
                // Handle other exceptions if needed
                return StatusCode(500); // Internal Server Error
            }
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(string id)
        {
            if (_context.OrderItems == null)
            {
                return NotFound();
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return (_context.OrderItems?.Any(e => e.OrderItemId == id)).GetValueOrDefault();
        }
    }
}
