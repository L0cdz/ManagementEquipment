using ManagementEquipment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;



namespace ManagementEquipment.Controllers
{
    public class LoginController : Controller
    {
        public static List<Account> accounts = new List<Account>();
        static MySqlConnection conn = null;
        static void Connection()
        {
            String connStr = "server=" +"localhost"+ ";" + "user=" +"root" + ";" + "database=" + "devicemanagement" + ";" + "password=" +""+ ";";
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
        static void listAccount()
        {

            ;
    
        }
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
         

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
      
            string key = (string)settingsReader.GetValue("SecurityKey",typeof(String));

            if (useHashing)
            {
               
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                

                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
         
            tdes.Key = keyArray;
          
          

            tdes.Mode = CipherMode.ECB;
         
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
                     
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public IActionResult Index()
        {
            return View();
        }
     
        public ActionResult PageAdmin()
        {
            return View();
        }
        public ActionResult PageUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyLogin(Account acc)
        {
            Connection();
            try
            {
                conn.Open();
                MySqlCommand mySqlCommand = new MySqlCommand("select *from account", conn);
                MySqlDataReader Reader = mySqlCommand.ExecuteReader();

                while (Reader.Read())
                {
            
                    int idd = Reader.GetInt32("id");
                    String name = Reader.GetString("Name");
                    String pwdd = Reader.GetString("Password");
                    String role = Reader.GetString("role");
                    accounts.Add(new Account { id = idd, Name = name, Password = pwdd, role = role });
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

            int id = acc.id;
            String pwd = acc.Password;
            MD5 mh = MD5.Create();
            //Chuyển kiểu chuổi thành kiểu byte
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pwd);
            //mã hóa chuỗi đã chuyển
            byte[] hash = mh.ComputeHash(inputBytes);
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < hash.Length; j++)
            {
                sb.Append(hash[j].ToString("x2"));
            }


            for (int i = 0; i < accounts.Count; i++)
            {
                
                if (accounts[i].id == id && sb.Equals(accounts[i].Password))
                {
                    
                    if (accounts[i].role.Equals("admin"))
                    {
                        return RedirectToAction("PageAdmin");
                       // return Json("admin");
                    }
                    else
                    {
                        return RedirectToAction("PageUser");
                      //  return Json("usser");

                    }
                }
                else
                {
                    ;
                }
               
            }
            return View();
            //Severity	Code	Description	Project	File	Line	Suppression State
            //Error CS0246  The type or namespace name 'AspNetCore' could not be found(are you missing a using directive or an assembly reference?)	ManagementEquipment C:\Users\Admin\Source\Repos\ManagementEquipment\ManagementEquipment\Controllers\LoginController.cs	9	Active


    }

}
}
