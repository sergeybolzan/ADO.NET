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
        //запрос о заказах клиентов - данные о клиенте (имя компании, контактное имя, адрес, город, страна, телефон), 
        //данные о заказе (дата заказа, дата поставки, стоимость перевозки, адрес поставки (город, страна, регион)), 
        //дополнительные данные о заказе (цена, количество, стоимость);
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = @"Data Source=(LocalDB)\v11.0;Initial Catalog=Northwind;Integrated Security=True";
                connection.Open();

                SqlCommand command = new SqlCommand("select companyname, contactname, address, city, country, phone from customers", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read() != false)
                {
                    Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);
                }






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
