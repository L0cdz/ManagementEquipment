using ManagementEquipment.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace ManagementEquipment.Controllers
{
    public class CategoryUserController : Controller
    {
        public static List<Equipment> listEquip = new List<Equipment>();
        static MySqlConnection conn = null;
        static void Connection()
        {
            String connStr = "server=" + "localhost" + ";" + "user=" + "root" + ";" + "database=" + "managementequip" + ";" + "password=" + "" + ";";
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                Console.WriteLine("Successfull");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult WatchEquip()
        {

            Connection();
            try
            {
                conn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("select *from equipment", conn);
                MySqlDataReader Reader = mySqlCommand.ExecuteReader();

                while (Reader.Read())
                {

                    int idd = Reader.GetInt32("id");
                    String name = Reader.GetString("name");
                    String des = Reader.GetString("description");
                    int qual = Reader.GetInt32("quality");
                    String img = Reader.GetString("imageUrl");
                    listEquip.Add(new Equipment { id = idd, name = name, description = des, quality = qual, imageUrl = img });

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                conn.Close();
            }


            return View(listEquip);
        }
    }

}
