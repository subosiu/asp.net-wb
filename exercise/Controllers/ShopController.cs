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
                    string sql = "SELECT * FROM Product WHERE pName LIKE @keyword OR Seller Like @keyword";
                    using (SqlCommand cmd = new SqlCommand(sql,conn))
                    { 
                        cmd.Parameters.AddWithValue("@keyword", "%" + (keyword??"") + "%");
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    pId = Convert.ToInt32(reader["pId"]),
                                    pName = reader["pName"].ToString(),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    Seller = reader["seller"].ToString(),
                                    Img = reader["img"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"])
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
        public IActionResult ProductDetail(int id)
        {
            string connectionString = _configuration.GetConnectionString("MyGoConnection");
            Product product = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Product WHERE pId = @pId"; 
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product = new Product
                                {
                                    pName = reader["pName"].ToString(),
                                    Price = Convert.ToInt32(reader["Price"]),
                                    Seller = reader["Seller"].ToString(),
                                    Img = reader["img"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    creTime = Convert.ToDateTime(reader["creTime"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品明細時發生錯誤");
                ViewBag.Error = "系統錯誤：" + ex.Message;
            }

            if (product == null)
                return NotFound(); // 找不到商品

            return View(product); // 傳單筆商品給 View
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
