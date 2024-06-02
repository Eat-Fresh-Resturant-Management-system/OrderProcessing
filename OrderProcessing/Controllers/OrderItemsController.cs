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

        public OrderItemsController(Order_Db context )
        {
            _context = context;         

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
        [HttpPost("CreatOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderItemDto orderDto)
        {
                     
           

            var order = new OrderItem
            {
                Price = orderDto.Price,
                Quantity = orderDto.Quantity,
            };


            _context.OrderItems.Add(order);


            await _context.SaveChangesAsync();

            return Ok("Order created successfully");
        }
    

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            

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
                                              
            }          
            catch (DbUpdateConcurrencyException ex)
            {
                
                var entry = ex.Entries.FirstOrDefault();
                if (entry != null)
                {
                    if (order.Price != null) { entry.CurrentValues.SetValues(order.Price); }
                    if (order.Quantity != null) { entry.CurrentValues.SetValues(order.Quantity); }                    
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
