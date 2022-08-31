using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestaoClientes.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();

        public String errorMessage = "";

        public String successMessage = "";

        public void OnGet()
        {
        }

        //executado quando submetemos o form com o botão submit
        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            //Validar os campos
            if(clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "Todos os campos são obrigatórios";
                return;
            }

            //Senão : Guardar os dados na base de dados


            try
            {
                //1º criamos a string de conexão
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                                "(name,email,phone, address) VALUES " +
                                "(@name, @email, @phone, @address);";

                    using(SqlCommand command = new SqlCommand(sql,connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address ="";
            successMessage = "Novo Cliente adicionado com sucesso";

            //após inserir com sucesso podemos redirecionar o utilizador para página clientes
            Response.Redirect("/Clients/Index");
        }
    }
}
