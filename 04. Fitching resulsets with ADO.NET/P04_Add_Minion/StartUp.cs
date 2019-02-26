using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P04_Add_Minion
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionInfo = Console.ReadLine().Split();

            var mName = minionInfo[1];
            var mAge = int.Parse(minionInfo[2]);
            var mTown = minionInfo[3];

            var villainInfo = Console.ReadLine().Split();

            var vName = villainInfo[1];

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringMinions))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int townId = GetTownId(connection, transaction, mTown);
                    InsertMinion(connection, transaction, mName, mAge, townId);
                    int mId = GetMinionsId(connection, transaction, mName, townId);
                    int vId = GetVillainId(connection, transaction, vName);
                    InsertMinionVillains(connection, transaction, mId, vId, mName, vName);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        private static void InsertMinionVillains(SqlConnection connection, SqlTransaction transaction, int mId, int vId, string mName, string vName)
        {
            string cmdTextMinions = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
            using (SqlCommand command = new SqlCommand(cmdTextMinions, connection, transaction))
            {
                command.Parameters.AddWithValue("@minionId", mId);
                command.Parameters.AddWithValue("@villainId", vId);

                command.ExecuteNonQuery();
            }

            Print($"Successfully added {mName} to be minion of {vName}.");
        }

        private static int GetMinionsId(SqlConnection connection, SqlTransaction transaction, string mName, int townId)
        {
            string cmdTextMinions = @"SELECT Id FROM Minions WHERE Name = @Name AND TownId = @townId";

            using (SqlCommand command = new SqlCommand(cmdTextMinions, connection, transaction))
            {
                command.Parameters.AddWithValue("@Name", mName);
                command.Parameters.AddWithValue("@townId", townId);

                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertMinion(SqlConnection connection, SqlTransaction transaction, string mName, int mAge, int townId)
        {
            string cmdTextMinions = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(cmdTextMinions, connection, transaction))
            {
                command.Parameters.AddWithValue("@name", mName);
                command.Parameters.AddWithValue("@age", mAge);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }

        private static int GetVillainId(SqlConnection connection, SqlTransaction transaction, string vName)
        {
            int? vId = null;

            string cmdTextVillain = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(cmdTextVillain, connection, transaction))
            {
                command.Parameters.AddWithValue("@Name", vName);
                vId = (int?)command.ExecuteScalar();

                if (vId == null)
                {
                    AddVillain(connection, transaction, vName);

                    vId = (int)command.ExecuteScalar();
                }
            }

            return (int)vId;
        }

        private static void AddVillain(SqlConnection connection, SqlTransaction transaction, string vName)
        {
            string cmdTextVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            ExecNonQuery(connection, transaction, cmdTextVillain, "@villainName", vName);

            Print($"Villain {vName} was added to the database.");
        }

        private static int GetTownId(SqlConnection connection, SqlTransaction transaction, string townName)
        {
            int? townId = null;

            string cmdTextMinions = @"SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(cmdTextMinions, connection, transaction))
            {
                command.Parameters.AddWithValue("@townName", townName);

                townId = (int?)command.ExecuteScalar();

                if (townId == null)
                {
                    AddTown(connection, transaction, townName);

                    townId = (int)command.ExecuteScalar();
                }
            }

            return (int)townId;
        }

        private static void AddTown(SqlConnection connection, SqlTransaction transaction, string townName)
        {
            string cmdTextMinions = @"INSERT INTO Towns (Name) VALUES (@townName)";

            ExecNonQuery(connection, transaction, cmdTextMinions, "@townName", townName);

            Print($"Town {townName} was added to the database.");
        }

        public static void Print(string text)
            => Console.WriteLine(text);

        private static void ExecNonQuery(SqlConnection connection, SqlTransaction transaction, string cmdText, string parameterName, string value)
        {
            using (SqlCommand command = new SqlCommand(cmdText, connection, transaction))
            {
                command.Parameters.AddWithValue(parameterName, value);
                command.ExecuteNonQuery();
            }
        }
    }
}
