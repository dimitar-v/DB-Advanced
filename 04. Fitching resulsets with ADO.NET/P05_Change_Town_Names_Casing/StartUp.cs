using P01_Initial_Setup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace P05_Change_Town_Names_Casing
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string countryName = Console.ReadLine();
            
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                int changedTownsCount = ChangeTownsName(connection, countryName);

                if (changedTownsCount > 0)
                {
                    string townsNames = GetTownsNames(connection, countryName);

                    Print($"{changedTownsCount} town names were affected.\n\r[{townsNames}]");
                }
                else
                {
                    Print("No town names were affected.");
                }
            }
        }

        private static string GetTownsNames(SqlConnection connection, string countryName)
        {
            string sql = @"SELECT t.Name 
                           FROM Towns as t
                           JOIN Countries AS c ON c.Id = t.CountryCode
                          WHERE c.Name = @countryName";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<string> towns = new List<string>();

                    while (reader.Read())
                    {
                        towns.Add((string)reader[0]);
                    }

                    return string.Join(", ", towns);
                } 
            }
        }

        private static int ChangeTownsName(SqlConnection connection, string countryName)
        {
            string sql = @"UPDATE Towns
                               SET Name = UPPER(Name)
                             WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryName);

                return command.ExecuteNonQuery();
            }
        }

        public static void Print(string text)
            => Console.WriteLine(text);
    }
}
