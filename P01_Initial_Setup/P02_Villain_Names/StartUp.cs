using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P02_Villain_Names
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                string cmdText = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                        FROM Villains AS v
                                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                    GROUP BY v.Id, v.Name
                                      HAVING COUNT(mv.VillainId) > 3
                                    ORDER BY COUNT(mv.VillainId)";

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = (string)reader["name"];
                                       //(string)reader[0];
                            var count = (int)reader["MinionsCount"];
                                        //(int)reader[1];

                            Console.WriteLine(name + " - " + count);
                        }
                    }
                }
            }
        }
    }
}
