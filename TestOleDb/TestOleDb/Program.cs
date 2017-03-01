using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOleDb
{
    class Program
    {
        static void Main(string[] args)
        {
            // -OleDb- SQL
            OleDbConnection connect = new OleDbConnection(); // можно через using
            try
            {
                connect.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Bol\DT\med.mdb";
                //connect.StateChange += connect_StateChange; == лямбда
                connect.StateChange += (os, ea) => { Console.WriteLine(ea.CurrentState); };
                connect.Open();
                //Console.WriteLine(connect.State);

                OleDbCommand cmdSelect = new OleDbCommand()
                {
                    Connection = connect,
                    CommandText = @"SELECT doctorName, doctorProfession FROM Doctors"
                };
                OleDbDataReader reader = cmdSelect.ExecuteReader();
                while(reader.Read())
                {
                    Console.WriteLine("{0} {1}", reader[0], reader[1]);
                    //Console.WriteLine("{0} {1}", reader["doctorName"], reader["doctorProfession"]);
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connect.Close();
            }

            Console.ReadKey();
        }

        //static void connect_StateChange(object sender, System.Data.StateChangeEventArgs e)
        //{
        //    Console.WriteLine(e.CurrentState);
        //}
    }
}
