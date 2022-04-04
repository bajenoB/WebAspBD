using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;

namespace WebAspBD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        public Sql_Operation sql_operat { get; set; }
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public ActionResult Login(string login, string pass)
        {
            if (login.Length > 4 && pass.Length > 6)
            {
                sql_operat = new Sql_Operation(new User(login, pass));
                if (sql_operat.Login())
                {
                    return StatusCode(200, "All good");
                }
                else
                {
                    return Problem("Incorrect login or pass");
                }
            }
            else
            {
                return StatusCode(400);
            }
        }
    }
}