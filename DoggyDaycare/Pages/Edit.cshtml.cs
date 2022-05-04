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
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public DaycareModel DaycareRecord { get; set; }

        public IActionResult OnGet(int id)
        {
            DaycareRecord = GetById(id);

            return Page();
        }

        private DaycareModel GetById(int id)
        {
            var DaycareRecord = new DaycareModel();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM Daycare WHERE Id = {id}";
                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    DaycareRecord.Id = reader.GetInt32(0);
                    DaycareRecord.Owner = reader.GetString(1);
                    DaycareRecord.Name = reader.GetString(2);
                    DaycareRecord.Date = DateTime.Parse(reader.GetString(3));
                    DaycareRecord.Duration = reader.GetInt32(4);
                }

                return DaycareRecord;
            }
        }

        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE Daycare SET Owner = '{DaycareRecord.Owner}', Name = '{DaycareRecord.Name}', date = '{DaycareRecord.Date}', duration = {DaycareRecord.Duration}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToPage("Index");
        }
    }
}
