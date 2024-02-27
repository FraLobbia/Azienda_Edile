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

        public ActionResult EmployeePaymentHistory(int id)
        {
            // Recupero i dati dalla query string
            int id_Employee = id;

            // Connessione al db tramite la stringa di connessione presente nel file Web.config     
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();

            // Creo la connessione al db tramite la stringa di connessione 
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("SELECT * FROM Pagamenti WHERE id_Employee = @id_Employee", conn);
            cmd.Parameters.AddWithValue("@id_Employee", id_Employee);

            List<Pagamento> pagamentiList = new List<Pagamento>();

            try
            {
                // Apro la connessione al db
                conn.Open();

                // Eseguo il comando sql
                SqlDataReader reader = cmd.ExecuteReader();

                // Leggo i dati dal db
                while (reader.Read())
                {
                    // Creo un oggetto di tipo Pagamento
                    Pagamento pagamento = new Pagamento(
                        Convert.ToInt32(reader["id_Pagamento"]),
                         Convert.ToDateTime(reader["PeriodoPagamento"]),
                          Convert.ToDecimal(reader["AmmontarePagamento"]),
                                            reader["TipoPagamento"].ToString(),
                            Convert.ToInt32(reader["id_Employee"])
                                                                                                                                       );

                    // Aggiungo l'oggetto alla lista che poi verrà passata alla view
                    pagamentiList.Add(pagamento);
                }
            }
            catch (Exception ex)
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close(); // Chiudo la connessione al db, NECESSARIO
            }


            return View(pagamentiList);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult CreateEmployee()
        {
            return View();
        }

        // POST: Employee/Create
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

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
