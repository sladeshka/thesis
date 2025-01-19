namespace SalesVentilationEquipment.Server
{
    public class AppSettings
    {
        public static string DBConnectionString { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            DBConnectionString = configuration["DEFAULT_CONNECTION"] ?? configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
            if (string.IsNullOrEmpty(DBConnectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
        }
    }
}
