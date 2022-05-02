using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoggyDaycare.Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace DoggyDaycare.Pages
{
	public class CreateModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public DaycareModel Daycare { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tablecmd = connection.CreateCommand();
                tablecmd.CommandText =
                    $"INSERT INTO Daycare(Owner, Name, Date, Duration) VALUES('{Daycare.Owner}','{Daycare.Name}','{Daycare.Date}',{Daycare.Duration})";
                tablecmd.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToPage("./index");
        }
    }
}
