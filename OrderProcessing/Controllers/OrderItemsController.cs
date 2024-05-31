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
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            _logger.LogInformation("UpdateOrder endpoint is called.");

            var order = await _context.OrderItems.FindAsync(id);
            if (order == null || orderItem == null)
            {
                return NotFound();
            }
            if (order.Price != null) { order.Price = orderItem.Price ?? order.Price; }
            else { order.Price = orderItem.Price; }
            if (order.Quantity != null) { order.Quantity = orderItem.Quantity ?? order.Quantity; }
            else { order.Quantity = orderItem.Quantity; }
            try
            {          await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated order with orderId {id}");                               
            }          
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("Concurrency conflict occurred while updating order: " + ex.Message);
                var entry = ex.Entries.FirstOrDefault();
                if (entry != null)
                {
                    if (order.Price != null) { entry.CurrentValues.SetValues(order.Price); }
                    if (order.Quantity != null) { entry.CurrentValues.SetValues(order.Quantity); }

                   _logger.LogInformation($"Concurrency conflict resolved for order with orderId {id}");         
                  return NoContent();
                }
            }
            return Ok();

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
