using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Roleplay.DatabaseAPI
{
    public class API
    {
        #region Settings
        public static readonly string HOST = "173.249.46.25";
        public static readonly int PORT = 3306;
        public static readonly string DATABASE = "testserver";
        public static readonly string USERNAME = "ragemp";
        public static readonly string PASSWORD = "NNhZkN6yTHgnuhs6";
        public static readonly string SSL_MODE = "none";

        // Die Anzahl an Verbindungen, die gleichzeitig offen sind.
        public static readonly int POOL_SIZE = 32;

        // Der Intervall (in Minuten) in dem die Verbindungen erneuert werden.
        public static readonly int RECONNECT_INTERVAL = 180;
        #endregion

        private static API instance;

        private List<MySqlConnection> availableConnections;

        private API()
        {
            this.availableConnections = new List<MySqlConnection>();

            PopulatePool();
            StartReconnectThread();
        }

        /// <summary>
        /// Gibt die Singleton-Instanz der DatabaseAPI zurück.
        /// </summary>
        /// <returns>Die Singleton-Instanz der DatabaseAPI.</returns>
        public static API GetInstance()
        {
            if (instance == null)
            {
                instance = new API();
            }

            return instance;
        }


        public static void executeNonQuery (MySqlCommand cmd)
        {
            MySqlConnection conn = GetInstance().GetConnection();
            cmd.Connection = conn;
            try
            {
                cmd.ExecuteNonQuery();
            } catch (MySqlException ex)
            {
                Console.WriteLine("DB ERROR: " + ex.ErrorCode + "|" + ex.Message);
                throw ex;
            } finally
            {
                GetInstance().FreeConnection(conn);
            }
        }

        
        /// <summary>
        /// Gibt eine Verbindung aus dem Pool zurück.
        /// </summary>
        /// <returns>Eine Verbindung aus dem Pool.</returns>
        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = null;

            do
            {
                Monitor.Enter(availableConnections);

                bool sleep = false;

                if (availableConnections.Count > 0)
                {
                    connection = availableConnections[0];

                    availableConnections.RemoveAt(0);
                }
                else
                {
                    sleep = true;
                }

                Monitor.Exit(availableConnections);

                if (sleep)
                {
                    Task.Delay(1).Wait();
                }
            } while (connection == null);

            return connection;
        }

        /// <summary>
        /// Übergibt eine Verbindung zurück an den Pool.
        /// </summary>
        /// <param name="connection">Die Verbindung, die an den Pool zurückgegeben wird.</param>
        public void FreeConnection(MySqlConnection connection)
        {

            //TODO: Entfernen. Ist nur zum testen
            if (IsReaderAttached(connection))
            {
                string errorMessage = "[DatabaseAPI] Datenbank-Reader nicht geschlossen! Letzte Stack-Frames:\n";

                StackFrame[] frames = new StackTrace().GetFrames();
                int interestingFrameCount = Math.Min(frames.Length, 10);

                for (int i = 0; i < interestingFrameCount; i++)
                {
                    StackFrame frame = frames[i];

                    errorMessage += ("Index " + i).PadRight(15) + "Class=" + frame.GetMethod().DeclaringType.FullName.PadRight(40) + "Method=" + frame.GetMethod().Name + "\n";
                }

                Console.WriteLine(errorMessage);
            }

            Monitor.Enter(availableConnections);

            availableConnections.Add(connection);

            Monitor.Exit(availableConnections);
        }

        /// <summary>
        /// Füllt den Pool mit Verbindungen.
        /// </summary>
        private void PopulatePool()
        {
            string connectionDescription = "Server=" + HOST + ";Database=" + DATABASE + ";Port=" + PORT + ";User Id=" + USERNAME + ";Password=" + PASSWORD + ";SSLMODE=" + SSL_MODE;

            for (int i = 0; i < POOL_SIZE; i++)
            {
                try
                {
                    MySqlConnection connection = new MySqlConnection(connectionDescription);

                    connection.Open();

                    availableConnections.Add(connection);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("[DatabaseAPI] Beim Erstellen der Pool-Population ist folgender Fehler aufgetreten: " + exc.Message);
                }
            }
        }

        /// <summary>
        /// Startet den Thread, der die Verbindungen des Pools in einem bestimmten Intervall erneuert.
        /// </summary>
        private void StartReconnectThread()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1000 * 60 * RECONNECT_INTERVAL).Wait();

                    int connectionsClosed = 0;

                    do
                    {
                        Monitor.Enter(availableConnections);

                        bool sleep = false;

                        if (availableConnections.Count > 0)
                        {
                            MySqlConnection connection = availableConnections[0];

                            availableConnections.RemoveAt(0);

                            connection.Close();
                            connectionsClosed++;
                        }
                        else
                        {
                            sleep = true;
                        }

                        Monitor.Exit(availableConnections);

                        if (sleep)
                        {
                            Task.Delay(1).Wait();
                        }
                    } while (connectionsClosed < POOL_SIZE);

                    Monitor.Enter(availableConnections);

                    PopulatePool();

                    Monitor.Exit(availableConnections);
                }
            });
        }


        private bool IsReaderAttached(MySqlConnection conn)
        {
            try
            {
                string sql = "SELECT 0;";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    rdr.Close();
                }
            }
            catch (Exception e)
            {
                return true;
            }

            return false;
        }
    }
}
