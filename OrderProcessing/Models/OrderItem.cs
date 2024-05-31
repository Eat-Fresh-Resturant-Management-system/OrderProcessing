using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.Models
{
    public class OrderItem
    {

        [Key]
        public int OrderItemId { get; set; }

        [ConcurrencyCheck]
      
        public int? Quantity { get; set; }
        public float? Price { get; set; }
        [Timestamp]
        public byte[]? ChangeCheck { get; set; }


    }
        public class OrderItemDto
    {

        public int Quantity { get; set; }
        public float Price { get; set; }


    }
}
