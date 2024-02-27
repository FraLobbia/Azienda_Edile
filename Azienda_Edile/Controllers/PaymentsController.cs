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
            return View();
        }

        // GET: Payments/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        public ActionResult Create(Pagamento pagamento)
        {
            try
            {
                // Recupero i dati dalla query string
                int id_Employee = Convert.ToInt32(Request.QueryString["id_Employee"]);

                // Ottengo i dati dal modello che ricevo in input
                DateTime PeriodoPagamento = pagamento.PeriodoPagamento;
                decimal AmmontarePagamento = pagamento.AmmontarePagamento;
                string TipoPagamento = pagamento.TipoPagamento;

                // Connessione al db tramite la stringa di connessione presente nel file Web.config     
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();

                // Creo la connessione al db tramite la stringa di connessione 
                SqlConnection conn = new SqlConnection(connectionString);

                try
                {
                    // Apro la connessione al db
                    conn.Open();

                    // Creo il comando sql da eseguire
                    SqlCommand cmd = new SqlCommand("INSERT INTO Pagamenti (PeriodoPagamento, AmmontarePagamento, TipoPagamento, id_Employee) VALUES (@PeriodoPagamento, @AmmontarePagamento, @TipoPagamento, @id_Employee)", conn);

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
                return View();
            }
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Payments/Edit/5
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

        // GET: Payments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Payments/Delete/5
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
