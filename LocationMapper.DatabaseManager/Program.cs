using Npgsql;
using System;

namespace LocationMapper.DatabaseManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Host=localhost;Username=UkcLogbookMapper;Password=qwerty;Database=Ukc";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Retrieve all rows
                using (var cmd = new NpgsqlCommand("SELECT * FROM crag", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(2));
                    }
                }
            }
        }
    }
}
