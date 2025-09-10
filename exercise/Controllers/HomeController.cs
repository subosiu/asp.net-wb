using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using exercise.Models;

namespace exercise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string connectionString = @"Server=C8659\SQLEXPRESS;Database=MyGo;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // �����A��ܵn�J���
        public IActionResult Index()
        {
            return View();
        }

        // �n�J POST �ʧ@
        [HttpPost]
        public IActionResult Login(string account, string pw)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pw))
            {
                ViewBag.Error = "�п�J�b���M�K�X";
                return View("Index");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM Member WHERE account=@account AND pw=@pw";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@pw", pw);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // �ɦV Shop/Shop.cshtml
                            return RedirectToAction("Shop", "Shop");
                        }
                        else
                        {
                            ViewBag.Error = "�b���αK�X���~";
                            return View("Index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "�t�ο��~�G" + ex.Message;
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
