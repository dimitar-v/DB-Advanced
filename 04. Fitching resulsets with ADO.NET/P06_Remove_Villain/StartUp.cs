using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P06_Remove_Villain
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int vId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                string vName = GetVillainName(connection, transaction, vId);

                if (vName is null)
                {
                    Print("No such villain was found.");
                    transaction.Rollback();
                    return;
                }

                try
                {
                    int minionsCount = ReleasedMinions(connection, transaction, vId);
                    DeleteVillain(connection, transaction, vId);

                    transaction.Commit();

                    Print($"{vName} was deleted.\n\r{minionsCount} minions were released.");
                }
                catch (Exception ex)
                {
                    Print($"Commit Exception Type: {ex.GetType()}");
                    Print($"  Message: {ex.Message}");

                    transaction.Rollback();
                }                
            }
        }

        private static void DeleteVillain(SqlConnection connection, SqlTransaction transaction, int vId)
        {
            string sql = @"DELETE FROM Villains
                            WHERE Id = @villainId";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", vId);

                command.ExecuteNonQuery();
            }
        }

        private static int ReleasedMinions(SqlConnection connection, SqlTransaction transaction, int vId)
        {
            string sql = @"DELETE FROM MinionsVillains 
                            WHERE VillainId = @villainId";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", vId);

                return command.ExecuteNonQuery();
            }
        }

        private static string GetVillainName(SqlConnection connection, SqlTransaction transaction, int vId)
        {
            string sql = "SELECT Name FROM Villains WHERE Id = @villainId";

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", vId);

                return (string)command.ExecuteScalar();
            }
        }

        public static void Print(string text)
            => Console.WriteLine(text);
    }
}
