using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLotDisconnectedLayer
{
    public class InventoryDALDisLayer
    {
        // Поля данных.
        private string cnString = string.Empty;
        private SqlDataAdapter dAdapt = null;

        public InventoryDALDisLayer(string connectionString)
        {
            cnString = connectionString;
            // Конфигурировать SqlDataAdapter.
            ConfigureAdapter(out dAdapt);
        }
        private void ConfigureAdapter(out SqlDataAdapter dAdapt)
        {
            // Создать адаптер и настроить SelectCommand.
            dAdapt = new SqlDataAdapter("Select * From Inventory", cnString);
            // Динамически получить остальные объекты команд
            // во время выполнения, используя SqlCommandBuilder.
            SqlCommandBuilder builder = new SqlCommandBuilder(dAdapt);
        }

        public DataTable GetAllInventory()
        {
            DataTable inv = new DataTable("Inventory");
            dAdapt.Fill(inv);
            return inv;
        }

        private void UpdateInventory(DataTable modifiedTable)
        {
            dAdapt.Update(modifiedTable);
        }

    }
}

