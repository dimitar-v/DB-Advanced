using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P09_Increase_Age_Stored_Procedure
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionId = int.Parse(Console.ReadLine()); ;

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                MinionGrowUp(connection, minionId);
                Print(GetMinionNameAndAge(connection, minionId));
            }
        }

        private static void MinionGrowUp(SqlConnection connection, int minionId)
        {
            string sql = "EXEC usp_GetOlder @id";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", minionId);
                command.ExecuteNonQuery();
            }
        }

        private static string GetMinionNameAndAge(SqlConnection connection, int minionId)
        {
            string sql = "SELECT Name, Age FROM Minions WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", minionId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    return (string)reader["Name"] + " - " + (int)reader["Age"] + " years old";
                }
            }
        }

        public static void Print(string text)
            => Console.WriteLine(text);
    }
}
