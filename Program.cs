using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Task_5_6.MyDB;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace Task_5_6
{

    class Program
    {
        static void testDB()
        {
            Console.WriteLine("***** Fun with Data Provider Factories *****\n");
            // Получить строку соединения и поставщика ид файла *.config.
            
            string cnStr = ConfigurationManager.AppSettings["cnStr"];
            string dp = ConfigurationManager.AppSettings["provider"];
            // Получить фабрику поставщиков.
            DbProviderFactory df = DbProviderFactories.GetFactory(dp);
            // Получить объект подключения.
            using (DbConnection cn = df.CreateConnection())
            {
                Console.WriteLine("Your connection object is а: {0}", cn.GetType().Name);
                cn.ConnectionString = cnStr;
                cn.Open();
                // Создать объект команды.
                DbCommand cmd = df.CreateCommand();

                SqlConnection sqlCn = new SqlConnection(cnStr);
                sqlCn.Open();
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM Inventory", sqlCn);
                
                Console.WriteLine("Your command object is а: {0}", cmd.GetType().Name);
                cmd.Connection = cn;
                cmd.CommandText = "SELECT TOP 100 * FROM Inventory";
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Your data reader object is а: {0}", reader.GetType().Name);
                    Console.WriteLine("\n***** Current Inventory *****");
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write("{0}\t", reader.GetName(i));

                    }
                    while (reader.Read())
                   {
                       Console.WriteLine();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write("{0}",reader.GetValue(i).ToString());
                        }
            
                        

                        //Console.WriteLine("-> Car #{0} is a {1}.", reader["CarID"], reader["Make"]);
                    }
                }
            }

            //Data Source=PALADINAMA\SQLEXPRESS;Initial Catalog=MyDb;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False
             
        }

        class Car
        {
            private int AutoIDNumber;

        }
       
        static void Main(string[] args)
        {
            //testDB();
            //string cnStr = ConfigurationManager.AppSettings["cnStr"];
            //SqlConnection connection = new SqlConnection(cnStr);
            //connection.Open();
            //StringBuilder query = new StringBuilder();
            //int count = 100000000;
            //for (int i = 0; i <= count; i++)
            //{
            //    query.Clear();
            //    query.Append("INSERT INTO Test(value) VALUES ");
            //    for (int j = 0; j < 800; j++, i++)
            //    {
            //        query.Append("(" + (count - i).ToString() + "),");
            //    }
            //    query.Append("(0)");
            //    SqlCommand cmd = new SqlCommand(query.ToString(), connection);
            //    cmd.ExecuteNonQuery();
            //    Console.WriteLine(i);
            //}
            //Console.WriteLine("String okey");
            

            Console.ReadLine();



            int ind = 0;

            Company myCompany = new Company();
            myCompany.OpenCompany();

        //STEP 1
            start:
            List<Center> centers = myCompany.GetCenters();
            for (int i = 0; i < centers.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, centers[i].Name);
            }

            int CenterNum = ReadInt("Enter center number: ", "Wrong number", i => i > 0 && i <= centers.Count);
            Center center = centers[CenterNum - 1];
            Console.WriteLine("Center: " + center.Name);

       //STEP 2 

            ind = 1;
            var availableCars = center.AvailableCars;
            foreach (var value in availableCars)
            {
                Console.WriteLine("{0}. {1}", ind++, value.Car.Model);
            }

            int carNum = ReadInt("Enter car number: ", "Wrong number", i => i > 0 && i <= availableCars.Count);
            AvailableCar currentCar = availableCars[carNum - 1];
            Console.WriteLine("Car: " + currentCar.Car.Model + " price: " + currentCar.Price);

        //STEP 3
            Order myOrder = new Order();
            myOrder.Car = currentCar.Car;
            myOrder.LeftToPay = currentCar.Price;
            myOrder.Client = FormRequest();
            myOrder.ReturnTime = TimeRequest();

            if (!center.CheckOrder(myOrder))
            {
                Console.WriteLine("Error in order");
                goto start;
            }

            //STEP 4
            Console.WriteLine("Choose Payment Method:");
            Console.WriteLine("1. Visa");
            Console.WriteLine("2. Cash");

            PaymentMethod paymentMethod = GetPaymentMethod(ReadInt("Enter payment method: ", "Wrong number", i => i > 0 && i <= 2));
            Console.WriteLine("Payment method: " + paymentMethod);
            if (!center.Pay(paymentMethod))
            {
                Console.WriteLine("Error in payment");
                goto start;
            }

            if (!center.RegisterOrder(myOrder))
            {
                Console.WriteLine("Order rejected :" + myOrder.OrderMessage);
                goto start;
            }
            
            centers[1].ReturnCar(myOrder.Client);
            Console.WriteLine("Car returned");
            goto start;
            



            Console.ReadLine();
        }

        static int ReadInt()
        {
            return ReadInt(string.Empty);
        }
        static int ReadInt(Predicate<int> comparer)
        {
            return ReadInt(string.Empty, string.Empty, comparer);
        }
        static int ReadInt(string message)
        {
            return ReadInt(message, string.Empty);
        }
        static int ReadInt(string message, string errorMessage)
        {
            return ReadInt(message, errorMessage, i => true);
        }
        static int ReadInt(string message, Predicate<int> comparer)
        {
            return ReadInt(message, string.Empty, comparer);
        }
        static int ReadInt(string message, string errorMessage, Predicate<int> comparer)
        {
            int ID = 0;

            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out ID) && comparer(ID))
                {
                    return ID;
                }
                Console.WriteLine(errorMessage);
            }
        }

        static PaymentMethod GetPaymentMethod(int paymentNum)
        {
            PaymentMethod paymentMethod;
            switch (paymentNum)
            {
                case 1:
                    paymentMethod = PaymentMethod.Visa;
                    break;
                case 2:
                    paymentMethod = PaymentMethod.Cash;
                    break;
                default:
                    paymentMethod = PaymentMethod.None;
                    break;
            }
            return paymentMethod;
        }
        static Client FormRequest()
        {
            Client clientForm = new Client();

            Console.WriteLine("Enter:");
            Console.Write("First Name: ");
            clientForm.FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            clientForm.LastName = Console.ReadLine();
            Console.Write("Pasport Identification Number: ");
            clientForm.PassportIdNumber = Console.ReadLine();

            return clientForm;
        }
        static DateTime TimeRequest()
        {
            DateTime time = new DateTime();

            while (true)
            {
                Console.Write("Enter Return Time: ");
                if (DateTime.TryParse(Console.ReadLine(), out time))
                {
                    return time;
                }
                Console.WriteLine("Wrong Time Format!!");
            }
        }
    }
}
