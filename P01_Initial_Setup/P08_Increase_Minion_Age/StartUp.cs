using P01_Initial_Setup;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace P08_Increase_Minion_Age
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionsIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                UpdateMinions(connection, minionsIds);

                Print(GetMinionsNameAndAge(connection));
            }
        }

        private static void UpdateMinions(SqlConnection connection, int[] minionsIds)
        {
            string sql = @"UPDATE Minions
                           SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                         WHERE Id = @Id";

            foreach (var mId in minionsIds)
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", mId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string GetMinionsNameAndAge(SqlConnection connection)
        {
            string sql = "SELECT Name, Age FROM Minions";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    StringBuilder sb = new StringBuilder();

                    while (reader.Read())
                    {
                        sb.Append((string)reader["Name"] + ' ' + (int)reader["Age"] + Environment.NewLine);
                    }

                    return sb.ToString().Trim();
                }
            }
        }

        public static void Print(string text)
            => Console.WriteLine(text);
    }
}
