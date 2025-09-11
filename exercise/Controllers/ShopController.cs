using exercise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace exercise.Controllers
{
    public class ShopController:Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly IConfiguration _configuration;
       
        public ShopController(ILogger<ShopController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Shop(string keyword)
        {
            string connectionString = _configuration.GetConnectionString("MyGoConnection");
            List<Product> products = new List<Product>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Product WHERE pName LIKE @keyword";
                    using (SqlCommand cmd = new SqlCommand(sql,conn))
                    { 
                        cmd.Parameters.AddWithValue("@keyword", "%" + (keyword??"") + "%");
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    pName = reader["pName"].ToString(),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    Selldata = reader["selldata"].ToString(),
                                    Img = reader["img"].ToString()
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品清單時發生錯誤");
                ViewBag.Error = "系統錯誤：" + ex.Message;
            }
            return View(products);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
