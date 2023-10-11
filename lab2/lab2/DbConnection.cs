using Microsoft.Extensions.Configuration;

namespace lab2
{
    public class DbConnection
    {
        public static DbConnection Instance { get => _instance; }
        private static DbConnection _instance;
        private string? _connectionString;
        private DbConnection()
        {
            string configFileName = "config.json";
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(configFileName, optional: false);

            IConfiguration config = builder.Build();
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        static DbConnection()
        {
            _instance = new DbConnection();
        }

        public string? GetConnectionString() => _connectionString;
    }
}