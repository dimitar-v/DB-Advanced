using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P03_Minion_Names
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                string cmdTextVillain = @"SELECT Name FROM Villains WHERE Id = @Id";
                               
                using (SqlCommand command = new SqlCommand(cmdTextVillain, connection))
                {
                    command.Parameters.AddWithValue("@Id", villainId);
                    var name = (string)command.ExecuteScalar();

                    if (name == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");

                        return;
                    }

                    Console.WriteLine("Villain: " + name);
                }

                string cmdTextMinions = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                   ORDER BY m.Name";

                using (SqlCommand command = new SqlCommand(cmdTextMinions, connection))
                {
                    command.Parameters.AddWithValue("@Id", villainId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long count = (long)reader["RowNum"];
                            var name = (string)reader["Name"];
                            var age = (int)reader["Age"];

                            Console.WriteLine($"{count}. {name} {age}");
                        }
                        
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                        }
                    }
                }
            }
        }
    }
}
