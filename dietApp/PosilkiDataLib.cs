using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace dietApp
{
    class PosilkiDataLib
    {
        public PosilkiDataLib()
        {

        }

        public async static void InitializeDB()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("database.db", CreationCollisionOption.OpenIfExists);
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");

            try
            {
                using (SqliteConnection conn = new SqliteConnection($"Filename={pathToDB}"))
                {
                    conn.Open();
                    string tableCommand = "CREATE TABLE IF NOT EXISTS posilki(" +
                        "posilekId INTEGER PRIMARY KEY, " +
                        "nazwa NVARCHAR(50) NOT NULL, " +
                        "kalorie INTEGER NOT NULL, " +
                        "bialka INTEGER NOT NULL," +
                        "tluszcze INTEGER NOT NULL," +
                        "weglowodany INTEGER NOT NULL," +
                        "typ NVARCHAR(10) NOT NULL, " +
                        "data NVARCHAR(50) NOT NULL)";

                    SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
                    createTable.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void addRecord(string nazwa, int kalorie, int bialka, int tluszcze, int weglowodany, string typ, string data)
        {
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");

            try
            {
                using (SqliteConnection conn = new SqliteConnection($"Filename={pathToDB}"))
                {
                    conn.Open();
                    SqliteCommand insert = new SqliteCommand();

                    insert.Connection = conn;

                    insert.CommandText = "INSERT INTO posilki VALUES (NULL, @nazwa, @kalorie, @bialka, @tluszcze, @weglowodany, @typ, @data);";
                    insert.Parameters.AddWithValue("@nazwa", nazwa);
                    insert.Parameters.AddWithValue("@kalorie", kalorie);
                    insert.Parameters.AddWithValue("@bialka", bialka);
                    insert.Parameters.AddWithValue("@tluszcze", tluszcze);
                    insert.Parameters.AddWithValue("@weglowodany", weglowodany);
                    insert.Parameters.AddWithValue("@typ", typ);
                    insert.Parameters.AddWithValue("@data", data);

                    insert.ExecuteReader();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class posilek
        {
            public posilek(string nazwa, int kalorie, int bialka, int tluszcze, int weglowodany, string typ, string data)
            {
                this.nazwa = nazwa;
                this.kalorie = kalorie;
                this.bialka = bialka;
                this.tluszcze = tluszcze;
                this.weglowodany = weglowodany;
                this.typ = typ;
                this.data = data;
            }

            public String nazwa { get; set; }
            public int kalorie { get; set;}
            public int bialka { get; set;}
            public int tluszcze { get; set; }
            public int weglowodany { get; set; }
            public String typ { get; set; }
            public String data { get; set; }
        }
           

        public static List<posilek> GetRecordsByData(string data)
        {
            List<posilek> posilkiList = new List<posilek>();

            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");
            using (SqliteConnection conn = new SqliteConnection($"Filename={pathToDB}"))
            {
                conn.Open();

                String select = $"SELECT nazwa, kalorie, bialka, tluszcze, weglowodany, typ, data FROM posilki WHERE data ='{data}';";
                SqliteCommand getRecord = new SqliteCommand(select, conn);

                SqliteDataReader reader = getRecord.ExecuteReader();

                while (reader.Read())
                {
                    posilkiList.Add(new posilek(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), 
                        reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5), reader.GetString(6)));
                }

                conn.Close();
            }

            return posilkiList;
        }

        public static List<posilek> GetRecordsByTypAndData(string typ, string data)
        {
            List<posilek> posilkiList = new List<posilek>();

            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");
            try
            {
                using (SqliteConnection conn = new SqliteConnection($"Filename={pathToDB}"))
                {
                    conn.Open();

                    String select = $"SELECT nazwa, kalorie, bialka, tluszcze, weglowodany, typ, data FROM posilki WHERE typ = '{typ}' and data = '{data}';";
                    SqliteCommand getRecord = new SqliteCommand(select, conn);

                    SqliteDataReader reader = getRecord.ExecuteReader();

                    while (reader.Read())
                    {
                        posilkiList.Add(new posilek(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), 
                            reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5), reader.GetString(6)));
                    }

                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            return posilkiList;
        }

        public static int GetKalorieByTypAndData(string typ, string data)
        {
            int kalorie = 0;

            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");
            try
            {
                using (SqliteConnection conn = new SqliteConnection($"Filename={pathToDB}"))
                {
                    conn.Open();

                    String select = $"SELECT SUM(kalorie) FROM posilki WHERE typ = '{typ}' and data = '{data}';";
                    SqliteCommand getRecord = new SqliteCommand(select, conn);

                    SqliteDataReader reader = getRecord.ExecuteReader();

                    while (reader.Read())
                    {
                        kalorie = reader.GetInt32(0);
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return kalorie;
        }

        public static List<int> GetKalorieByData(string data)
        {
            List<int> kalorie = new List<int>();

            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");
            try
            {
                using (SqliteConnection conn = new SqliteConnection($"Filename={pathToDB}"))
                {
                    conn.Open();

                    String select = $"SELECT SUM(kalorie), SUM(bialka), SUM(tluszcze), SUM(weglowodany) FROM posilki WHERE data = '{data}';";
                    SqliteCommand getRecord = new SqliteCommand(select, conn);

                    SqliteDataReader reader = getRecord.ExecuteReader();

                    while (reader.Read())
                    {
                        kalorie.Add(reader.GetInt32(0));
                        kalorie.Add(reader.GetInt32(1));
                        kalorie.Add(reader.GetInt32(2));
                        kalorie.Add(reader.GetInt32(3));

                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return kalorie;
        }

    }
}
