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


        public ClientsModel.ClientInfo clientInfo = new ClientsModel.ClientInfo();


        [Required] [Display(Name = "Email Address")]
        public string Username = "";

        [Required] [DataType(DataType.Password)]
        public string Password = "";


        public IActionResult OnPost()
        {
            Username = Request.Form["Username"];
            Password = Request.Form["Password"];

            //var isValidUser =
            //    Username == "a@aa"
            //    && Password == "123";
            if (Username != "" && Password != "")
            {
                String connectionString =
                    "Data Source=discordit.database.windows.net;Initial Catalog=discord;Persist Security Info=True;User ID=beks;Password=J5cmRZt4i42B2yX";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT username from users WHERE username = @username ;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", Username);

                        command.ExecuteNonQuery();

                        Response.Redirect("/Clients");
                    }
                }

                //var isValidUser =
                //    Username == Username
                //    && Password == "123";
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
            return Page();
        }

    }

}