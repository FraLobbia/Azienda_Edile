using Azienda_Edile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Azienda_Edile.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            List<Employee> employeesList = Employee.GetEmployees();

            return View(employeesList);
        }


        public ActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(Employee employee)
        {
            // Connessione al db tramite la stringa di connessione presente nel file Web.config     
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();

            // Creo la connessione al db tramite la stringa di connessione 
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                // Apro la connessione al db
                conn.Open();

                // Creo il comando sql da eseguire
                SqlCommand cmd = new SqlCommand("INSERT INTO Employee (Nome, Cognome, Indirizzo, CodiceFiscale, Coniugato, NumeroFigliACarico, Mansione) VALUES (@Nome, @Cognome, @Indirizzo, @CodiceFiscale, @Coniugato, @NumeroFigliACarico, @Mansione)", conn);

                // Aggiungo i parametri al comando sql
                cmd.Parameters.AddWithValue("@Nome", employee.Nome);
                cmd.Parameters.AddWithValue("@Cognome", employee.Cognome);
                cmd.Parameters.AddWithValue("@Indirizzo", employee.Indirizzo);
                cmd.Parameters.AddWithValue("@CodiceFiscale", employee.CodiceFiscale);
                cmd.Parameters.AddWithValue("@Coniugato", employee.Coniugato);
                cmd.Parameters.AddWithValue("@NumeroFigliACarico", employee.NumeroFigliACarico);
                cmd.Parameters.AddWithValue("@Mansione", employee.Mansione);

                // Eseguo il comando sql
                cmd.ExecuteNonQuery();


            }
            // Gestione dell'eccezione
            catch (Exception ex) // ex è l'oggetto che rappresenta l'eccezione
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close(); // Chiudo la connessione al db, NECESSARIO
            }
            return View();
        }





    }
}