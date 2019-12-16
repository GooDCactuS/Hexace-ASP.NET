using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hexace.Models
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; } = "Game description page.";

        public AboutModel()
        {

        }

        public void OnGet()
        {

        }
    }
}