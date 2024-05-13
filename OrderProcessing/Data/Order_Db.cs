using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using OrderProcessing.Models;

namespace OrderProcessing.Data
{
    public class Order_Db : DbContext
    {
        public Order_Db(DbContextOptions<Order_Db> options)
        : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<TableData> TableDatas { get; set; }


    }
}
