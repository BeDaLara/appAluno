using MySql.Data.MySqlClient;


namespace appEscola
{
    public class Program
    {
        private static string connectingString = "Server=localhost;Database=db_livros;Uid=root;Pwd=1234567;SslMode=none;";


        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Adicionar Aluno");
                Console.WriteLine("2 - Listar Aluno");
                Console.WriteLine("3 - Editar Aluno");
                Console.WriteLine("4 - Excluir Aluno");
                Console.WriteLine("5 - Sair");
                Console.Write("Escolha uma opção acima: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarAluno();
                        break;
                    case "2":
                        ListarAluno();
                        break;
                    case "3":
                        Editar();
                        break;
                    case "4":
                        Excluir();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
        }
        static void AdicionarAluno()
        {
            Console.Write("Informe o nome do Aluno : ");
            string nome = Console.ReadLine();

            Console.Write("Informe a idade do Aluno: ");
            int idade = int.Parse(Console.ReadLine());

            Console.Write("Informe o curso do Aluno : ");
            string curso = Console.ReadLine();

            Console.Write("Quando o aluno foi matriculado: ");
            string matricula = Console.ReadLine();



            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "INSERT INTO aluno(nome,idade,curso,data_matricula) VALUES(@nome,@idade,@curso,@data_matricula)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@idade", idade);
                cmd.Parameters.AddWithValue("@curso", curso);
                cmd.Parameters.AddWithValue("@data_matricula", matricula);

                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Aluno cadastrado com sucesso");
        }
        static void ListarAluno()
        {
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "SELECT  id_aluno, nome, idade, curso, data_matricula FROM aluno";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["id_aluno"]}, Nome: {reader["nome"]}, Idade: {reader["idade"]}, Curso: {reader["curso"]}, Data da matricula: {reader["data_matricula"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe Aluno cadastrado ");
                    }
                }
            }
        }
        static void Excluir()
        {
            Console.Write("Informe o Id do Aluno que deseja excluir: ");
            int idExclusao = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "DELETE FROM aluno WHERE id_aluno=@Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idExclusao);

                int linhaAfetada = cmd.ExecuteNonQuery();

                if (linhaAfetada > 0)
                {
                    Console.WriteLine("Aluno excluido com sucesso");
                }
                else
                {
                    Console.WriteLine("Aluno não encontrado");
                }
            }
        }
        static void Editar()
        {
            Console.Write("Informe o Id do Aluno que deseja editar: ");
            int idEditar = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "SELECT * FROM aluno WHERE id_aluno=@Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idEditar);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.Write("Informe o novo nome do Aluno: (*Deixe o campo em branco para não alterar): ");
                        string novoNome = Console.ReadLine();

                        Console.Write("Informe a nova idade do Aluno: (*Deixe o campo em branco para não alterar): ");
                        string novoIdade = Console.ReadLine();

                        Console.Write("Informe o novo curso do Aluno: (*Deixe o campo em branco para não alterar): ");
                        string novoCurso = Console.ReadLine();

                        Console.Write("Informe a nova data de quando o aluno foi matriculado: (*Deixe o campo em branco para não alterar): ");
                        string novoMatricula = Console.ReadLine();


                        reader.Close();

                        string queryUpdate = "UPDATE aluno SET nome = @nome, idade=@idade, curso=@curso, data_matricula=@data_matricula WHERE id_aluno=@Id";

                        cmd = new MySqlCommand(queryUpdate, connection);

                        cmd.Parameters.AddWithValue("@nome", string.IsNullOrWhiteSpace(novoNome) ? reader["nome"] : novoNome);
                        cmd.Parameters.AddWithValue("@idade", string.IsNullOrWhiteSpace(novoIdade) ? reader["idade"] :int.Parse(novoIdade));
                        cmd.Parameters.AddWithValue("@curso", string.IsNullOrWhiteSpace(novoCurso) ? reader["curso"] :novoCurso);
                        cmd.Parameters.AddWithValue("@data_matricula", string.IsNullOrWhiteSpace(novoMatricula) ? reader["data_matricula"] : novoMatricula);

                        cmd.Parameters.AddWithValue("@Id", idEditar);

                        cmd.ExecuteNonQuery();  
                        Console.WriteLine("O aluno foi atualizado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("O Id do aluno informado não existe!");

                    }
                }
            }

        }

    }
}
