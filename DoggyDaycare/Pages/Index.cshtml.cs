﻿using DoggyDaycare.Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace DoggyDaycare.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    public List<DaycareModel> Records { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        Records = GetTodaysRecords();
    }

    private List<DaycareModel> GetTodaysRecords()
    {
        using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString"))) {
            connection.Open();
            var tablecmd = connection.CreateCommand();
            tablecmd.CommandText =
                $"SELECT * FROM Daycare";

            var tableData = new List<DaycareModel>();
            SqliteDataReader reader = tablecmd.ExecuteReader();

            while(reader.Read())
            {
                if(DateTime.Parse(reader.GetString(3)).Date == DateTime.Today)
                {
                    tableData.Add(
                        new DaycareModel
                        {
                            Id = reader.GetInt32(0),
                            Owner = reader.GetString(1),
                            Name = reader.GetString(2),
                            Date = DateTime.Parse(reader.GetString(3)),
                            Duration = reader.GetInt32(4)
                        }
                    );
                }
            }
            return tableData;
        }
    }
}

