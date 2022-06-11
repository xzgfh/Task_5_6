using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLotConnectedLayer
{
    public class NewCar
    {
        public int CarID { get; set; }
        public string Color { get; set; }
        public string Make { get; set; }
        public string PetName { get; set; }
    }

    public class InventoryDAL
    {
        private SqlConnection sqlCn = null;
        public void OpenConnection(string connectionString)
        {
            sqlCn = new SqlConnection();
            sqlCn.ConnectionString = connectionString;
            sqlCn.Open();
        }
        public void CloseConnection()
        {
            sqlCn.Close();
        }

        public void InsertAuto(int id, string color, string make, string petName)
        {
            // Сформировать SQL-оператор.
            string sql = string.Format("INSERT INTO Inventory " + 
                                       "(CarID, Make, Color, PetName) Values" + 
                                       "(@CarID, @Make, @Color, @PetName)");

            // Выполнить SQL-оператор с применением нашего подключения.
            using(SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                // Заполнить коллекцию параметров.
                cmd.Parameters.Add(new SqlParameter("@CarID",   SqlDbType.Int)      { Value = id });
                cmd.Parameters.Add(new SqlParameter("@Make",    SqlDbType.Char, 10) { Value = make });
                cmd.Parameters.Add(new SqlParameter("@Color",   SqlDbType.Char, 10) { Value = color });
                cmd.Parameters.Add(new SqlParameter("@PetName", SqlDbType.Char, 10) { Value = petName });

                cmd.ExecuteNonQuery();
            }

        }
        public void InsertAuto(NewCar car)
        {
            // Сформировать SQL-оператор.
            string sql = string.Format("INSERT INTO Inventory" + " (CarID, Make, Color, PetName) VALUES" +
                                       "('{0}', '{1}', '{2}', '{3}')", car.CarID, car.Make, car.Color, car.PetName);
            // Выполнить SQL-оператор с применением нашего подключения.
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                cmd.ExecuteNonQuery();
            }
        }
       
        public void DeleteCar(int id)
        {
        // Получить идентификатор удаляемого автомобиля, затем выполнить удаление.
            string sql = string.Format("DELETE FROM Inventory WHERE CarID = {0}", id);
            using(SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Sorry! That car is on order!", ex);
                }
            }
        }
        public void UpdateCarPetName(int id, string newPetName)
        {
            // Получить идентификатор модифицируемого автомобиля и новое дружественное имя.
            string sql = string.Format("UPDATE Inventory SET PetName = {0} WHERE CarID = {1}", newPetName, id);
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetAllInventoryAsDataTable()
        {
            // Здесь будут храниться записи.
            DataTable inv = new DataTable();
            // Подготовить объект команды.
            string sql = "SELECT * FROM Inventory";
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                // Заполнить DataTable данными из объекта чтения и выполнить очистку.
                inv.Load(dr);
                dr.Close();
            }
            return inv;
        }

        public string LookUpPetName(int carID)
        {
            string carPetName = string.Empty;
            // Установить имя хранимой процедуры.
            using (SqlCommand cmd = new SqlCommand("GetPetName", this.sqlCn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // Входной параметр.
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@carID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = carID;
                // По умолчанию параметры считаются входными (Input) , но все же для ясности:
                param.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(param);

                // Выходной параметр.
                param = new SqlParameter();
                param.ParameterName = "@petName";
                param.SqlDbType = SqlDbType.Char;
                param.Size = 10;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                // Выполнить хранимую процедуру.
                cmd.ExecuteNonQuery();
                // Возвратить выходной параметр.
                carPetName = (string) cmd.Parameters["@petName"].Value;
            }
            return carPetName;
        }


    }
}