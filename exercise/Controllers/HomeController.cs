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

        // �����A��ܵn�J���
        public IActionResult Index()
        {
            return View();
        }

        // �n�J POST �ʧ@
        [HttpPost]
        public IActionResult Login(string account, string pw,string status)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pw))
            {
                ViewBag.Error = "�п�J�b���M�K�X";
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
