using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hexace.Models
{
    public class StatisticsModel : PageModel
    {
        public Dictionary<string, int> Statistics { get; set; }

        public StatisticsModel(Dictionary<string, int> stats)
        {
            Statistics = stats;
        }

        public void OnGet()
        {
           

        }
    }
}