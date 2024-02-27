using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace Azienda_Edile.Models
{
    public class Pagamento
    {
        public int id_Pagamento { get; set; }
        public int id_Employee { get; set; }
        public DateTime PeriodoPagamento { get; set; }
        public decimal AmmontarePagamento { get; set; }
        public string TipoPagamento { get; set; }

        public Pagamento()
        {
        }

        public Pagamento(DateTime PeriodoPagamento, decimal AmmontarePagamento, string TipoPagamento, int id_Employee)
        {
            this.PeriodoPagamento = PeriodoPagamento;
            this.AmmontarePagamento = AmmontarePagamento;
            this.TipoPagamento = TipoPagamento;
            this.id_Employee = id_Employee;
        }

        public Pagamento(int id_Pagamento, DateTime PeriodoPagamento, decimal AmmontarePagamento, string TipoPagamento, int id_Employee)
        {
            this.id_Pagamento = id_Pagamento;
            this.PeriodoPagamento = PeriodoPagamento;
            this.AmmontarePagamento = AmmontarePagamento;
            this.TipoPagamento = TipoPagamento;
            this.id_Employee = id_Employee;
        }

        // Metodo statico per ottenere la lista dei pagamenti 
        // Non riceve parametri in input
        // Restituisce una lista di oggetti di tipo Pagamento
        public static List<Pagamento> GetPagamenti()
        {
            List<Pagamento> pagamentiList = new List<Pagamento>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pagamenti", conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pagamento pagamento = new Pagamento(
                        Convert.ToInt32(reader["id_Pagamento"]),
                       Convert.ToDateTime(reader["PeriodoPagamento"]),
                        Convert.ToDecimal(reader["AmmontarePagamento"]),
                                          reader["TipoPagamento"].ToString(),
                          Convert.ToInt32(reader["id_Employee"])
                    );

                    pagamentiList.Add(pagamento);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return pagamentiList;
        }

        // Metodo statico per ottenere la lista dei pagamenti di un dipendente
        // Riceve in input l'id del dipendente
        // Restituisce una lista di oggetti di tipo Pagamento riferiti al dipendente
        public static List<Pagamento> GetListPaymentsByEmployee(int id)
        {
            List<Pagamento> pagamentiList = new List<Pagamento>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pagamenti WHERE id_Employee = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pagamento pagamento = new Pagamento(
                            Convert.ToDateTime(reader["PeriodoPagamento"]),
                             Convert.ToDecimal(reader["AmmontarePagamento"]),
                                               reader["TipoPagamento"].ToString(),
                               Convert.ToInt32(reader["id_Employee"])
                            );

                    pagamentiList.Add(pagamento);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return pagamentiList;
        }

        // Metodo statico per ottenere un oggetto di tipo Employee tramite l'id del pagamento
        // Riceve in input l'id del pagamento
        // Restituisce un oggetto di tipo Employee
        public static Employee GetEmployeeByPaymentId(int id)
        {
            Employee employee = new Employee();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Employee " +
                "WHERE id_Employee = (" +
                "SELECT id_Employee FROM Pagamenti " +
                "WHERE id_Pagamento = @id)", conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employee.id_Employee = Convert.ToInt32(reader["id_Employee"]);
                    employee.Nome = reader["Nome"].ToString();
                    employee.Cognome = reader["Cognome"].ToString();
                    employee.Indirizzo = reader["Indirizzo"].ToString();
                    employee.CodiceFiscale = reader["CodiceFiscale"].ToString();
                    employee.Coniugato = Convert.ToBoolean(reader["Coniugato"]);
                    employee.NumeroFigliACarico = Convert.ToInt32(reader["NumeroFigliACarico"]);
                    employee.Mansione = reader["Mansione"].ToString();

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return employee;
        }

        // Metodo statico per ottenere un oggetto di tipo Pagamento tramite l'id del pagamento
        // Riceve in input l'id del pagamento
        // Restituisce un oggetto di tipo Pagamento
        public static Pagamento GetPaymentById(int id)
        {
            Pagamento pagamento = new Pagamento();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pagamenti WHERE id_Pagamento = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pagamento.id_Pagamento = Convert.ToInt32(reader["id_Pagamento"]);
                    pagamento.PeriodoPagamento = Convert.ToDateTime(reader["PeriodoPagamento"]);
                    pagamento.AmmontarePagamento = Convert.ToDecimal(reader["AmmontarePagamento"]);
                    pagamento.TipoPagamento = reader["TipoPagamento"].ToString();
                    pagamento.id_Employee = Convert.ToInt32(reader["id_Employee"]);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return pagamento;
        }
    }
}