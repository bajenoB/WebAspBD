using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;



namespace WebAspBD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddToDateBaseController : ControllerBase
    {
        private readonly ILogger<AddToDateBaseController> _logger;
        public Sql_Operation sql_operat { get; set; }
        public AddToDateBaseController(ILogger<AddToDateBaseController> logger)
        {
            _logger = logger;

        }
        [HttpPost]
        public StatusCodeResult AddUser(string login, string pass, string email, int year, int month, int day)
        {
            if (DateTime.DaysInMonth(year, month) < day)
            {
                return StatusCode(400);
            }
            else
            {
                sql_operat = new Sql_Operation(new User(login, pass, new DateTime(year, month, day), email, DateTime.Now));

                if (sql_operat.user.ValidDates())
                {
                    if (sql_operat.AddUser(sql_operat.user))
                    {
                        return StatusCode(200);
                    }
                    else
                    {
                        return StatusCode(400);
                    }
                }
                else
                {
                    return StatusCode(400);
                }
            }
        }
    }
}