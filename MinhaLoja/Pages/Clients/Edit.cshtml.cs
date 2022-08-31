using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestaoClientes.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();

        public String errorMessage = "";

        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                //conexão a base de dados
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //query para lermos dados da tabela clientes
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    //executa a query
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);  
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //le dados da tabela
                            if (reader.Read())
                            {             
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
            }
        }

        //Aqui vamos implementar os dados que recebemos do form
        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            //Se qql campo estiver vazio, devemos mostrar uma mensagem de erro
            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "Todos os campos são obrigatórios";
                return;
            }

            try
            {
                //conexão a base de dados
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //query para dar update dos dados da tabela clientes
                    String sql = "UPDATE clients " +
                                 "SET name=@name, email=@email, phone=@phone, address = @address" + 
                                 "WHERE id=@id";
                    //executa a query
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                            command.Parameters.AddWithValue("@name", clientInfo.name);
                            command.Parameters.AddWithValue("@email", clientInfo.email);
                            command.Parameters.AddWithValue("@phone", clientInfo.phone);
                            command.Parameters.AddWithValue("@address", clientInfo.address);
                            command.Parameters.AddWithValue("@id", clientInfo.id);

                            command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }

            //após inserir com sucesso podemos redirecionar o utilizador para página clientes
            Response.Redirect("/Clients/Index");
        }
    }
}
