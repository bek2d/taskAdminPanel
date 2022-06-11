using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstWebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            var EmailAddress = "";
            var Password = "";
            EmailAddress = Request.Form["EmailAddress"];
            Password = Request.Form["Password"];
            if (EmailAddress != "" && Password != "")
            {
                string connectionString =
                    "Data Source=discordit.database.windows.net;Initial Catalog=discord;Persist Security Info=True;User ID=beks;Password=J5cmRZt4i42B2yX";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT username FROM Users WHERE username = @EmailAddress";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailAddress", EmailAddress);
                        var result = command.ExecuteScalar().ToString();
                        if (result != null)
                        {
                            Response.Redirect("/About");
                        }
                    }
                    
                }
                

            }

            
                //var isValidUser =
                //EmailAddress == "a@aa"
                //&& Password == "aA1!";

            //if (!isValidUser)
            //{
            //    ModelState.AddModelError("", "Invalid username or password!");
            //}

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, EmailAddress) },
                    scheme
                )
            );

            return SignIn(user, scheme);
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}