using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ1
{
    class Program
    {
        static void RunQuery(OleDbCommand command, string cmdText)
        {
            command.CommandText = cmdText;
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read() != false)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + " ");
                }
                Console.WriteLine();
            }
            reader.Close();
        }
        static void Main(string[] args)
        {
            OleDbConnection connect = new OleDbConnection();
            try
            {
                connect.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=../../../med.mdb";
                connect.StateChange += (os, ea) => { Console.WriteLine(ea.CurrentState); };
                connect.Open();
                OleDbCommand command = connect.CreateCommand();

                Console.WriteLine("Полная информация о пациентах больницы:");
                RunQuery(command, "SELECT * FROM Patients");

                Console.WriteLine("\nПолная информация о врачах в больницы:");
                RunQuery(command, "SELECT * FROM Doctors");

                Console.WriteLine("\nСписок пациентов с указанием их лечащих врачей:");
                RunQuery(command, "SELECT patientName, doctorName FROM (Patients INNER JOIN Visits ON Patients.patientID = Visits.visitPatient) INNER JOIN Doctors ON Doctors.doctorID = Visits.visitDoctor");

                Console.WriteLine("\nВывести информацию о стоимостях посещений пациентами врачей (фамилия пациента, фамилия врача, стоимость):");
                RunQuery(command, "SELECT Patients.patientName, Doctors.doctorName, VisitCosts.visitCostValue FROM (((Patients INNER JOIN Visits ON Patients.patientID = Visits.visitPatient) INNER JOIN Doctors ON Doctors.doctorID = Visits.visitDoctor) INNER JOIN VisitCosts ON Visits.visitID = VisitCosts.visitCostID)");

                Console.WriteLine("\nВывести информацию о поставленных диагнозах за период (фамилия врача, диагноз, дата):");
                RunQuery(command, "SELECT Doctors.doctorName, Visits.visitComment, VisitCosts.visitCostTill FROM ((Doctors INNER JOIN Visits ON Doctors.doctorID = Visits.visitDoctor) INNER JOIN VisitCosts ON Visits.visitID = VisitCosts.visitCostID)");

                Console.WriteLine("\nВывести информацию о самом дорогом визите к врачу (фамилия пациента, фамилия врача, стоимость):");
                RunQuery(command, "SELECT Patients.patientName, Doctors.doctorName, VisitCosts.visitCostValue FROM (((VisitCosts LEFT OUTER JOIN Visits ON VisitCosts.visitCostID = Visits.visitID) LEFT OUTER JOIN Patients ON Patients.patientID = Visits.visitPatient) LEFT OUTER JOIN Doctors ON Doctors.doctorID = Visits.visitDoctor) WHERE (VisitCosts.visitCostValue = (SELECT MAX(visitCostValue) AS Expr1 FROM VisitCosts VisitCosts_1))");

                Console.WriteLine("\nВывести информацию о количестве пациентов каждого врача (фамилия врача и количество пациентов):");
                RunQuery(command, "SELECT A.doctorName, COUNT(B.visitPatient) AS Expr1 FROM (Doctors A LEFT OUTER JOIN (SELECT DISTINCT visitDoctor, visitPatient FROM Visits) B ON A.doctorID = B.visitDoctor) GROUP BY A.doctorName");

                Console.WriteLine("\nВывести информацию о суммарной стоимости посещений каждого врача (фамилия врача и сумма):");
                RunQuery(command, "SELECT Doctors.doctorName, SUM(VisitCosts.visitCostValue) AS Expr1 FROM ((Doctors LEFT OUTER JOIN Visits ON Doctors.doctorID = Visits.visitDoctor) LEFT OUTER JOIN VisitCosts ON Visits.visitID = VisitCosts.visitCostID) GROUP BY Doctors.doctorName");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connect.Close();
            }
            Console.ReadKey();
        }
    }
}
