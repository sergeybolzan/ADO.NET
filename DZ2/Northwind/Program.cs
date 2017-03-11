using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind
{
    class Program
    {
        static void RunQuery(SqlConnection connection, string cmdText)
        {
            SqlCommand command = new SqlCommand(cmdText, connection);
            SqlDataReader reader = command.ExecuteReader();
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
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = @"Data Source=(LocalDB)\v11.0;Initial Catalog=Northwind;Integrated Security=True";
                connection.Open();

                // запрос о заказах клиентов - данные о клиенте (имя компании, контактное имя, адрес, город, страна, телефон), данные о заказе (дата заказа, дата поставки, стоимость перевозки, адрес поставки (город, страна, регион)), дополнительные данные о заказе (цена, количество, стоимость)
                RunQuery(connection, "SELECT companyname, contactname, address, city, country, phone, orderdate, shippeddate, freight, shipcountry, shipcity, shipregion, UnitPrice, Quantity FROM Customers JOIN Orders ON Customers.customerID = Orders.customerID JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID");
                Console.WriteLine("\n\n\n\n");

                // запрос о продуктах, цены которых в интервале от 10 до 60 и которые в настоящий момент  не сняты с производства. В запросе должна быть информация (Название продукта, цена, снято/не снято,  количество в упаковке, имя категории)
                RunQuery(connection, "SELECT ProductName, UnitPrice, QuantityPerUnit, CategoryName FROM Products JOIN Categories ON Products.CategoryID = Categories.CategoryID WHERE UnitPrice BETWEEN 10 AND 60 AND ReorderLevel > 0");
                Console.WriteLine("\n\n\n\n");

                // запрос, формирующий информацию о количестве клиентов в городе 
                RunQuery(connection, "SELECT City, COUNT(City) FROM Customers GROUP BY City");
                Console.WriteLine("\n\n\n\n");

                // запрос, выводящий информацию о клиентах  с указанием общей суммы заказов за заданный период: Необходимо вывести: имя компании,  город, страна,  дата заказа, сумма.
                RunQuery(connection, "SELECT CompanyName, City, Country, OrderDate, SUM(UnitPrice) FROM Customers JOIN Orders ON Customers.CustomerID = Orders.CustomerID JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID GROUP BY CompanyName, City, Country, OrderDate");
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
