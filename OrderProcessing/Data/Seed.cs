

namespace OrderProcessing.Data;

public class SeedData
{
    public static void Seed(Order_Db dbContext)
    {
        dbContext.Database.EnsureCreated();
    }
}