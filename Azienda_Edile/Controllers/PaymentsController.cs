using Azienda_Edile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Azienda_Edile.Controllers
{
    public class PaymentsController : Controller
    {
        // GET: Payments
        public ActionResult Index()
        {
            // ottengo la lista dei pagamenti tramite il metodo static
            // GetPagamenti() del modello Pagamento
            List<Pagamento> pagamentiList = Pagamento.GetPagamenti();

            // restituisco la vista con la lista dei pagamenti passata come parametro
            return View(pagamentiList);
        }


        // GET: Payments/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0) // question: perchè se il percorso è Payments/Details e quindi senza id non funziona?
            {
                return View();
            }
            // ottengo la lista dei pagamenti tramite il metodo static GetPaymentById() del modello Pagamento
            Pagamento payment = Pagamento.GetPaymentById(id);

            // restituisco la vista con la lista dei pagamenti passata come parametro
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            // perchè con percorso Payments/Create senza nessun ID ho bisogno di ritornare la view Create??
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        public ActionResult Create(int id, Pagamento FormPagamento)
        {
            try
            {
                int id_Employee = id;

                // Ottengo i dati dal modello che ricevo in input
                DateTime PeriodoPagamento = FormPagamento.PeriodoPagamento;
                decimal AmmontarePagamento = FormPagamento.AmmontarePagamento;
                string TipoPagamento = FormPagamento.TipoPagamento;

                // Connessione al db tramite la stringa di connessione presente nel file Web.config     
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();

                // Creo la connessione al db tramite la stringa di connessione 
                SqlConnection conn = new SqlConnection(connectionString);

                try
                {
                    // Apro la connessione al db
                    conn.Open();

                    // Creo il comando sql da eseguire
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Pagamenti " +
                        "(PeriodoPagamento, AmmontarePagamento, TipoPagamento, id_Employee)" +
                        " VALUES " +
                        "(@PeriodoPagamento, @AmmontarePagamento, @TipoPagamento, @id_Employee)", conn);

                    // Aggiungo i parametri al comando sql
                    cmd.Parameters.AddWithValue("@PeriodoPagamento", PeriodoPagamento);
                    cmd.Parameters.AddWithValue("@AmmontarePagamento", AmmontarePagamento);
                    cmd.Parameters.AddWithValue("@TipoPagamento", TipoPagamento);
                    cmd.Parameters.AddWithValue("@id_Employee", id_Employee);

                    // Eseguo il comando sql
                    cmd.ExecuteNonQuery();
                }
                // Gestione dell'eccezione
                catch (Exception ex) // ex è l'oggetto che rappresenta l'eccezione
                {
                    Response.Write("Errore");
                    Response.Write(ex.Message);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Payments/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Payments/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            // Ottengo i dati dal form
            DateTime PeriodoPagamento = Convert.ToDateTime(collection["PeriodoPagamento"]);
            decimal AmmontarePagamento = Convert.ToDecimal(collection["AmmontarePagamento"]);
            string TipoPagamento = collection["TipoPagamento"];
            // e l'id dal parametro
            int id_Payment = id;

            // Ottengo l'oggetto Employee tramite il metodo static GetEmployeeByPaymentId() del modello Pagamento
            Employee employee = Pagamento.GetEmployeeByPaymentId(id_Payment);
            //ottengo l'id dell'employee
            int id_Employee = employee.id_Employee;

            try
            {
                conn.Open();

                // Metto in una stringa il comando SQL da eseguire
                string query = "UPDATE Pagamenti " +
                    "SET PeriodoPagamento = @PeriodoPagamento, AmmontarePagamento = @AmmontarePagamento, TipoPagamento = @TipoPagamento, id_Employee = @id_Employee " +
                    "WHERE id_Pagamento = @id_Payment";

                // Creo il comando SQL passando la stringa del comando e la connessione al db
                SqlCommand cmd = new SqlCommand(query, conn);

                // Aggiungo i parametri al comando SQL 
                cmd.Parameters.AddWithValue("@PeriodoPagamento", PeriodoPagamento);
                cmd.Parameters.AddWithValue("@AmmontarePagamento", AmmontarePagamento);
                cmd.Parameters.AddWithValue("@TipoPagamento", TipoPagamento);
                cmd.Parameters.AddWithValue("@id_Payment", id_Payment);
                cmd.Parameters.AddWithValue("@id_Employee", id_Employee);


                // Eseguo il comando SQL
                cmd.ExecuteNonQuery();

                // Gestione dell'eccezione
            }
            catch (Exception ex)
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            // Ritorno alla vista Index dopo aver eseguito l'update
            return RedirectToAction("Index");
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // Creo il comando SQL da eseguire
                SqlCommand cmd = new SqlCommand("DELETE FROM Pagamenti WHERE id_Pagamento = @id", conn);

                // Aggiungo i parametri al comando SQL
                cmd.Parameters.AddWithValue("@id", id);

                // Eseguo il comando SQL
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View();
        }


    }
}
