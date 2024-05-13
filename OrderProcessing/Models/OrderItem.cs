using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.Models
{
    public class OrderItem
    {

        [Key]
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }


    }
        public class OrderItemDto
    {

        public int Quantity { get; set; }
        public float Price { get; set; }


    }
}
