using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Task_5_6.MyDB;
using System.Data.Common;
namespace Task_5_6
{

    public class Center
    {
        private Company company;
        private static uint _ID = 1;
        public uint ID { get; private set; }
        public string Name { get; private set; }

        public List<HistoryCar> HistoryCars { get; private set; }
        public List<AvailableCar> AvailableCars { get; private set; }
        public List<BusyCar> BusyCars { get; private set; }


        public Center(string name, Company company)
        {
            
            this.Name = name;
            this.ID = _ID++;
            this.company = company;

            HistoryCars = new List<HistoryCar>();
            AvailableCars = new List<AvailableCar>();
            BusyCars = new List<BusyCar>();
        }
        public void OpenCenter(List<AvailableCar> cars)
        {
            AvailableCars.AddRange(cars);
        }

        public bool RegisterOrder(Order order)
        {
            if (CheckOrder(order))
            {
                order.OrderStatus = OrderStatus.Confirmed;
                order.OrderMessage = null;
                ConfirmOrder(order);
                return true;
            }
            else
            {
                order.OrderStatus = OrderStatus.Reject;
                order.OrderMessage = "Reject because....";
                return false;
            }
        }
        public bool CheckOrder(Order order)
        {
            if (!AvailableCars.Exists(i => i.Car.ID == order.Car.ID))
                return false;
            if (order.Client.FirstName == "123")
                return false;
            //Check client information (first, last name, passport and etc.)
            
            return true;
        }
        private void ConfirmOrder(Order order)
        {
            AvailableCars.RemoveAll(i => i.Car.ID == order.Car.ID);

            BusyCar busyCar = new BusyCar()
            {
                Car = order.Car,
                Client = order.Client,
                StartTime = DateTime.Now,
                FromCenter = this
            };
            BusyCars.Add(busyCar);
            company.ConfirmOrder(busyCar);
        }

        public void CloseOrder(Client client, Center fromCenter)
        {
            int ind = BusyCars.FindIndex(i => i.Client.ID == client.ID);
            if (ind == -1) return;
            BusyCar busyCar = BusyCars[ind]; 
            BusyCars.RemoveAt(ind);
            HistoryCars.Add(new HistoryCar()
            {
                Car = busyCar.Car,
                Client = busyCar.Client,
                StartTime = busyCar.StartTime,
                EndTime = DateTime.Now,
                FromCenter = fromCenter,  
                ToCenter = this

            }); 
        }
        public void ReturnCar(Client client)
        {
            BusyCar busyCar = company.BusyCars.Find(i => i.Client.ID == client.ID);
          
            AvailableCars.Add(new AvailableCar()
            {
                Car = busyCar.Car,
                Price = 100
            });
            company.CloseOrder(client, this);

        }

        public bool Pay(PaymentMethod paymentMethod)
        {
            switch (paymentMethod)
            {
                case PaymentMethod.Visa:
                    return VisaPay();

                case PaymentMethod.Cash:
                    return CashPay();

                default:
                    return false;
            }
        }
        private bool VisaPay()
        {
            return true;
        }
        private bool CashPay()
        {
            return true;
        }



    }
}
