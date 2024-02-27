using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
namespace Azienda_Edile.Models
{
    public class Employee
    {
        public int id_Employee { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }
        public bool Coniugato { get; set; }
        [Display(Name = "Numero figli a carico")]
        public int NumeroFigliACarico { get; set; }
        public string Mansione { get; set; }

        public Employee()
        {
        }
        public Employee(int id, string nome, string cognome, string indirizzo, string codiceFiscale, bool coniugato, int numeroFigliACarico, string mansione)
        {
            id_Employee = id;
            Nome = nome;
            Cognome = cognome;
            Indirizzo = indirizzo;
            CodiceFiscale = codiceFiscale;
            Coniugato = coniugato;
            NumeroFigliACarico = numeroFigliACarico;
            Mansione = mansione;
        }

        public static List<Employee> GetEmployees()
        {
            List<Employee> employeesList = new List<Employee>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee", conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee(
                        Convert.ToInt32(reader["id_Employee"]),
                                        reader["Nome"].ToString(),
                                        reader["Cognome"].ToString(),
                                        reader["Indirizzo"].ToString(),
                                        reader["CodiceFiscale"].ToString(),
                      Convert.ToBoolean(reader["Coniugato"]),
                        Convert.ToInt32(reader["NumeroFigliACarico"]),
                                        reader["Mansione"].ToString());

                    // Aggiungo l'oggetto alla lista che poi verrà passata alla view
                    employeesList.Add(employee);
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
            return employeesList;
        }
    }
}