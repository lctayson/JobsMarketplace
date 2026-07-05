namespace JobsMarketplace.API.Constants;

public class CacheKeys
{
    public static class Jobs
    {
        public static string ById(int id) => $"job:{id}";
    }

    public static class Customers
    {
        public static string ById(int id) => $"customer:{id}";
    }

    public static class Contractors
    {
        public static string ById(int id) => $"contractor:{id}";
    }
}
