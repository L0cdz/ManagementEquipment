using Microsoft.AspNetCore.Mvc;
using ManagementEquipment.Models;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Collections;
using PagedList;
using Hangfire.Dashboard;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Xml.Linq;


namespace ManagementEquipment.Controllers
{
    public class CategoryAdminController : Controller
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
    
       
        [HttpGet]
        public ActionResult ManagementEquip()
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
   
        public ActionResult DeleteEquip(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteEquip(Equipment eq)
        {
            Connection();

            try
            {
                conn.Open();
                String query = "DELETE FROM `equipment` WHERE id=" + eq.id;
                MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("ManagementEquip");
        }
        public ActionResult AddEquip()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEquip(Equipment eq)
        {
            Connection();

            try
            {
                conn.Open();
                String query = "INSERT INTO `equipment`(`name`, `description`, `quality`, `imageUrl`) VALUES (" + "'" + eq.name + "'" + "," + "'" + eq.description + "'" + "," + "'" + eq.description + "'" + ","+"'" + eq.imageUrl + "'"+")";
                MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("ManagementEquip");
        }
        public ActionResult EditEquip(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditEquip(Equipment eq)
        {
            Connection();
            try
            {
                conn.Open();
                String query = "UPDATE `equipment` SET `name`=" + "'" + eq.name + "'" + "," + "`description`=" + "'" + eq.description + "'" + "," + "`quality`=" + "'" + eq.quality + "'" + "," + "`imageUrl`=" + "'" + eq.imageUrl + "'" + "WHERE id=" + eq.id;
                MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("ManagementEquip");
        }
    }
}
