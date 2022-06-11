using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FirstWebApp.Pages.ClientsModel;

namespace FirstWebApp.Pages
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();

        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.registration_date = Request.Form["registration_date"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 ||
                clientInfo.registration_date.Length == 0)
            {
                errorMessage = "Required";
                    return;
            }

            try
            {
                String connectionString ="Data Source=discordit.database.windows.net;Initial Catalog=discord;Persist Security Info=True;User ID=beks;Password=J5cmRZt4i42B2yX";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients" + "(name, email, phone, registration_date) VALUES" +
                                 "(@name, @email, @phone, @registration_date);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@registration_date", clientInfo.registration_date);

                        command.ExecuteNonQuery();

                        Response.Redirect("/Clients");
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            //clear clientInfo
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.registration_date = "";
            //return success message
            successMessage = "Client created successfully";
        }
    }
}
