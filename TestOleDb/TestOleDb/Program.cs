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
            using (OleDbConnection connect = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\Bolzan\Study\GitHub\ADO.NET\med.mdb"))
            {
                connect.StateChange += (os, ea) => { Console.WriteLine(ea.CurrentState); };
                connect.Open();
                OleDbCommand command = new OleDbCommand(@"SELECT * FROM Patients", connect);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0}. {1} {2} - {3}", reader["patientID"], reader["patientFirstName"], reader["patientName"], reader["patientBirthDate"]);
                }
                reader.Close();

                Console.WriteLine();

                command.CommandText = @"SELECT * FROM Doctors";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0}. {1} {2} - {3}", reader["doctorID"], reader["doctorFirstName"], reader["doctorName"], reader["doctorProfession"]);
                }
                reader.Close();

                command.CommandText = @"SELECT patientName, doctorName FROM Patients INNER JOIN Visits ON Patients.patientID = Visits.visitPatient INNER JOIN Doctors ON Doctors.doctorID = Visits.visitDoctor";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0} {1}", reader["patientName"], reader["doctorName"]);
                }
                reader.Close();


            }
            Console.ReadKey();
        }
    }
}
