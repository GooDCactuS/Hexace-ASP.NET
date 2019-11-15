using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols;

namespace Hexace.Pages
{
    public class StatisticsModel : PageModel
    {
        public List<int> Statistics { get; set; } = new List<int>();
        

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

            Statistics.Add(350);
            Statistics.Add(220);
            Statistics.Add(470);
        }
    }
}