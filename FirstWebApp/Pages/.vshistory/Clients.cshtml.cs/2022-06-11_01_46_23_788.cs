using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstWebApp.Pages
{
    [Authorize]
    public class ClientsModel : PageModel
    {
        //get list of ClientInfo
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public void OnGet()
        {
            //connect to database
            try
            {   
                string connectionString = "Data Source=tcp:razormvcit.database.windows.net;Initial Catalog=itransitionproject;User ID=beks;Password=J5cmRZt4i42B2yX";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT id, name, email, registration_date, last_login, status FROM [Table] ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo client = new ClientInfo();
                                client.id = reader.GetInt32(0);
                                client.name = reader.GetString(1);
                                client.email= reader.GetString(2);
                                client.registration_date = reader.GetString(3);
                                client.last_login = reader.GetString(4);
                                client.status = reader.GetString(5);
                                listClients.Add(client);

                            }
                        }
                    }
                }
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        public class ClientInfo
        {
            public int id;
            public string name;
            public string email;
            public string password;
            public string registration_date;
            public string last_login;
            public string status;
        }
        }
}
