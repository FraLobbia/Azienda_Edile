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
    }
}