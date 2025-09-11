using exercise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace exercise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // 首頁，顯示登入表單
        public IActionResult Index()
        {
            return View();
        }

        // 登入 POST 動作
        [HttpPost]
        public IActionResult Login(string account, string pw,string status)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pw))
            {
                ViewBag.Error = "請輸入帳號和密碼";
                return View("Index");
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("MyGoConnection");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM Member WHERE a_id=@account AND pw=@pw AND status = @status";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@pw", pw);
                        cmd.Parameters.AddWithValue("@status", status);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // 導向 Shop/Shop.cshtml
                            return RedirectToAction("Shop", "Shop");
                        }
                        else
                        {
                            ViewBag.Error = "帳號或密碼錯誤";
                            return View("Index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "系統錯誤：" + ex.Message;
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
