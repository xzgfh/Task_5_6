using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5_6
{
    
    public class Client
    {
        private Action<int> buyAction;
        private static uint _ID = 1;
        public uint ID { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportIdNumber { get; set; }

        public Client()
        {
            this.ID = _ID++;
        }

        public void BuyCar()
        {
            

        }
    }
}
