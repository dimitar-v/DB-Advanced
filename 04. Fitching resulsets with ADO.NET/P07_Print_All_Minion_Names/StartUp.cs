using P01_Initial_Setup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace P07_Print_All_Minion_Names
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            List<string> minions = new List<string>();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                string sql = @"SELECT Name FROM Minions";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add((string)reader[0]);
                        }
                    }
                }
            }

            int last = minions.Count - 1;
            for (int i = 0; i < minions.Count / 2; i++)
            {
                Print(minions[i]);
                Print(minions[last--]);
            }

            if (minions.Count % 2 == 1)
            {
                Print(minions[last]);
            }
        }

        public static void Print(string text)
            => Console.WriteLine(text);
    }
}
