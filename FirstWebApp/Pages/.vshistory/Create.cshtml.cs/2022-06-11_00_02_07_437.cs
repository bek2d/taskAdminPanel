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
            clientInfo.password = Request.Form["password"];
            clientInfo.registration_date = DateTime.Today.ToString();

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.password.Length == 0)
            {
                errorMessage = "Required";
                    return;
            }

            try
            {
                String connectionString = "Data Source=tcp:razormvcit.database.windows.net;Initial Catalog=itransitionproject;User ID=beks;Password=J5cmRZt4i42B2yX";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO [Table]" + "(name, email, password, registration_date) VALUES" +
                                 "(@name, @email, @password, @registration_date);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@registration_date", clientInfo.registration_date);
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@password", clientInfo.password);

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
            clientInfo.password = "";
            //return success message
            successMessage = "Client created successfully";
        }
    }
}
