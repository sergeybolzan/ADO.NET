﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOleDb
{
    class Program
    {
        static void RunQuery(OleDbConnection connection, string cmdText)
        {
            OleDbCommand command = new OleDbCommand(cmdText, connection);
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
                connect.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=../../../med.mdb";
                connect.StateChange += (os, ea) => { Console.WriteLine(ea.CurrentState); };
                connect.Open();

                Console.WriteLine("Полная информация о пациентах больницы:");
                RunQuery(connect, "SELECT * FROM Patients");

                Console.WriteLine("\nПолная информация о врачах в больницы:");
                RunQuery(connect, "SELECT * FROM Doctors");

                Console.WriteLine("\nСписок пациентов с указанием их лечащих врачей:");
                RunQuery(connect, "SELECT patientName, doctorName FROM (Patients INNER JOIN Visits ON Patients.patientID = Visits.visitPatient) INNER JOIN Doctors ON Doctors.doctorID = Visits.visitDoctor");

                Console.WriteLine("\nВывести информацию о стоимостях посещений пациентами врачей (фамилия пациента, фамилия врача, стоимость):");
                RunQuery(connect, "SELECT Patients.patientName, Doctors.doctorName, VisitCosts.visitCostValue FROM (((Patients INNER JOIN Visits ON Patients.patientID = Visits.visitPatient) INNER JOIN Doctors ON Doctors.doctorID = Visits.visitDoctor) INNER JOIN VisitCosts ON Visits.visitID = VisitCosts.visitCostID)");

                Console.WriteLine("\nВывести информацию о поставленных диагнозах за период (фамилия врача, диагноз, дата):");
                RunQuery(connect, "SELECT Doctors.doctorName, Visits.visitComment, VisitCosts.visitCostTill FROM ((Doctors INNER JOIN Visits ON Doctors.doctorID = Visits.visitDoctor) INNER JOIN VisitCosts ON Visits.visitID = VisitCosts.visitCostID)");

                Console.WriteLine("\nВывести информацию о самом дорогом визите к врачу (фамилия пациента, фамилия врача, стоимость):");
                RunQuery(connect, "SELECT Patients.patientName, Doctors.doctorName, MAX(VisitCosts.visitCostValue) AS Expr1 FROM (((Doctors INNER JOIN Visits ON Doctors.doctorID = Visits.visitDoctor) INNER JOIN Patients ON Visits.visitPatient = Patients.patientID) INNER JOIN VisitCosts ON Visits.visitID = VisitCosts.visitCostID) GROUP BY Patients.patientName, Doctors.doctorName");

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
