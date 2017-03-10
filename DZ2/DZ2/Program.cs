using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ2
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = @"Data Source=(LocalDB)\v11.0;Initial Catalog=Northwind;Integrated Security=True";
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT companyname, contactname, address, city, country, phone, orderdate, shippeddate, freight, shipcountry, shipcity, shipregion, UnitPrice, Quantity FROM Customers JOIN Orders ON Customers.customerID = Orders.customerID JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read() != false)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + " ");
                    }
                    Console.WriteLine("\n");
                }
                reader.Close();



                Console.WriteLine("\n\n\n\n");



                command = new SqlCommand("SELECT ProductName, UnitPrice, QuantityPerUnit, CategoryName FROM Products JOIN Categories ON Products.CategoryID = Categories.CategoryID WHERE UnitPrice BETWEEN 10 AND 60 AND ReorderLevel > 0", connection);
                reader = command.ExecuteReader();
                while (reader.Read() != false)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + " ");
                    }
                    Console.WriteLine();
                }
                reader.Close();



                Console.WriteLine("\n\n\n\n");



                command = new SqlCommand("SELECT City, COUNT(City) FROM Customers GROUP BY City", connection);
                reader = command.ExecuteReader();
                while (reader.Read() != false)
                {
                    Console.WriteLine("город: {0}, количество клиентов: {1}", reader[0], reader[1]);
                }
                reader.Close();



                Console.WriteLine("\n\n\n\n");



                command = new SqlCommand("SELECT CompanyName, City, Country, OrderDate, SUM(UnitPrice) FROM Customers JOIN Orders ON Customers.CustomerID = Orders.CustomerID JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID GROUP BY CompanyName, City, Country, OrderDate", connection);
                reader = command.ExecuteReader();
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
            catch (SqlException ex)
            {
                string errors = ex.Message + "\n";
                foreach (SqlError error in ex.Errors)
                {
                    errors += "Message: " + error.Message + "\n" +
                              "Class: " + error.Class + "\n" +
                              "Procedure: " + error.Procedure + "\n" +
                              "LineNumber: " + error.LineNumber + "\n";
                }
                Console.WriteLine(errors);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            Console.ReadKey();

        }
    }
}
