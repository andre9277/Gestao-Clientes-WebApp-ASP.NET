using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//Para podermos conectar a base de dados
using System.Data.SqlClient;

namespace GestaoClientes.Pages.Clients
{
    //é o Model
    public class IndexModel : PageModel
    {

        //Lista para armazenar dados de vários clientes
        public List<ClientInfo> listClients = new List<ClientInfo>();

        public void OnGet()
        {
            try
            {
                //conexão a base de dados
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //query para lermos dados da tabela clientes
                    String sql = "SELECT * FROM clients";
                    //executa a query
                    using(SqlCommand command = new SqlCommand(sql,connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {   
                            //le dados da tabela
                            while(reader.Read())
                            {
                                //guardar dados neste objeto
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone= reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToString();

                                //adicionamos o nosso objeto a lista:
                                listClients.Add(clientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    //Classe que permite armazenar os dados de 1 cliente
    public class ClientInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
    }
}
