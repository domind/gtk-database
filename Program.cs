using System;
using Gtk;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
namespace window
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            win.Resize(400, 400);
            string connectionString =  //enter below det for your mysql database
         "Server=localhost;" +
         "Database=test;" +
         "User ID=test;" +
        // "Password=mypassword;" +
                "Pooling=false; Convert Zero Datetime=True; Allow Zero Datetime=True;";
            IDbConnection dbcon;
            dbcon = new MySqlConnection(connectionString);
            try
            {
                dbcon.Open();
            }
            catch
            {
                
                System.Environment.Exit(1);
            }

            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql =

           "SELECT * " +
           "FROM test";                  // database table
            dbcmd.CommandText = sql;
            IDataReader reader = dbcmd.ExecuteReader();
            Gtk.TreeView tree = new Gtk.TreeView();
            List<System.Type> colTypes = new List<System.Type>();
  
            List<string> rowValues = new List<string>();
            try
            {
              reader.Read();
                for (int col = 0; col < reader.FieldCount; col++)
                {
                    tree.AppendColumn(reader.GetName(col).ToString(), new Gtk.CellRendererText(), "text", col);
                    colTypes.Add(typeof(string));
                    rowValues.Add(reader[reader.GetName(col).ToString()].ToString());
                }
                ListStore resultListStore = new ListStore(colTypes.ToArray());
                resultListStore.AppendValues(rowValues.ToArray());
                rowValues.Clear();
                while (reader.Read())
                {
                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        rowValues.Add(reader[reader.GetName(col).ToString()].ToString());
                    }
                    resultListStore.AppendValues(rowValues.ToArray());
                    rowValues.Clear();

                }
            
                tree.Model = resultListStore;
                reader.Close();
                reader = null;
            }
            catch
            {
                System.Environment.Exit(1);//"ERROR: Could not read records"
            }
            dbcmd.Dispose();
            dbcmd = null;
  
            win.Add(tree);
            win.ShowAll();
            Application.Run();
        }
    }
}
