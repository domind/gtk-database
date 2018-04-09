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
            string connectionString =
         "Server=localhost;" +
         "Database=dom;" +
         "User ID=domin;" +
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
           "FROM dom";
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
/*                        for (int col = 0; col < reader.FieldCount; col++)
                        {
                            tree.AppendColumn(reader.GetName(col).ToString(), new Gtk.CellRendererText(), "text", col);
                            colTypes.Add(typeof(string));//reader.GetFieldType(col));
                        }
                            //Console.Write("{0,-11}", reader.GetName(col).ToString() + " ");
                      //  Console.WriteLine();
                        for (int col = 0; col < reader.FieldCount; col++)
                        {
                            if (reader.GetDataTypeName(col).ToString() == "INT")
                                Console.Write("number     ");
                            if (reader.GetDataTypeName(col).ToString() == "DOUBLE")
                                Console.Write("decimal    ");
                            if (reader.GetDataTypeName(col).ToString() == "DATE")
                                Console.Write("date       ");
                            if (reader.GetDataTypeName(col).ToString() == "TIME")
                                Console.Write("time       ");
                            if ((reader.GetDataTypeName(col).ToString() == "VARCHAR") || (reader.GetDataTypeName(col).ToString() == "TEXT"))
                                Console.Write("text       ");
                        }
                        Console.WriteLine();*/


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