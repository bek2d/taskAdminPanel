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
        public string email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public IActionResult OnPost()
        {
            var i = 0;
            var email = "";
            var password = "";
            email = Request.Form["email"];
            password = Request.Form["password"];
            if (email != "" && password != "")
            {
                string connectionString =
                    "Data Source=tcp:razormvcit.database.windows.net;Initial Catalog=itransitionproject;User ID=beks;Password=J5cmRZt4i42B2yX";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT email FROM [Table] WHERE email = @email";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        try
                        {
                            var result = command.ExecuteScalar().ToString();

                        }

                        catch
                        {
                            i = 1;
                            ModelState.AddModelError("", "Invalid username or password!");
                            Response.Redirect("/Create");
                        }

                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT email FROM [Table] WHERE password = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@password", password);
                        try
                        {
                            var result = command.ExecuteScalar().ToString();
                        }

                        catch
                        {
                            i = 1;
                            ModelState.AddModelError("", "Invalid username or password!");
                            Response.Redirect("/Create");


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
                        new[] { new Claim(ClaimTypes.Name, email) },
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