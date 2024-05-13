using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderProcessing.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderTime { get; set; }
    public string Status { get; set; }
}

