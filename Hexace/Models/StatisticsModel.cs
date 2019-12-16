using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hexace.Models
{
    public class StatisticsModel : PageModel
    {
        public List<int> Statistics { get; set; } = new List<int>();

        public StatisticsModel()
        {
            Statistics.Add(350);
            Statistics.Add(220);
            Statistics.Add(470);
        }

        public void OnGet()
        {
            //string connectionString = ConfigurationManager<>.ConnectionStrings["DefaultConnection"].ConnectionString;
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand { Connection = connection };
            //    command.CommandText = $"SELECT * ";
            //    var reader = command.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {

            //        }
            //    }
            //}

        }
    }
}