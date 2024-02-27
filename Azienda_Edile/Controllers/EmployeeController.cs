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
            // Connessione al db tramite la stringa di connessione presente nel file Web.config     
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString.ToString();

            // Creo la connessione al db tramite la stringa di connessione 
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee", conn);

            // Creo una lista di oggetti di tipo Employee
            List<Employee> employeesList = new List<Employee>();

            try
            {
                // Apro la connessione al db
                conn.Open();

                // Eseguo il comando sql
                SqlDataReader reader = cmd.ExecuteReader();

                // Leggo i dati dal db
                while (reader.Read())
                {
                    // Creo un oggetto di tipo Employee
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
            catch (Exception ex)
            {
                Response.Write("Errore");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close(); // Chiudo la connessione al db, NECESSARIO
            }

            //List<Employee> employeesList = new List<Employee>();
            //employeesList.Add(new Employee("Mario", "Rossi", "Via Roma", "RSSMRA80", true, 2, "Muratore"));
            //employeesList.Add(new Employee("Luca", "Bianchi", "Via Milano", "BNCLCU80", false, 0, "Capo Cantiere"));
            //employeesList.Add(new Employee("Paolo", "Verdi", "Via Torino", "VRDPL80", true, 3, "Manovale"));
            //employeesList.Add(new Employee("Giovanni", "Neri", "Via Napoli", "NRIGNN80", false, 0, "Muratore"));
            //employeesList.Add(new Employee("Giuseppe", "Gialli", "Via Firenze", "GLLGPP80", true, 1, "Piastrellista"));

            return View(employeesList);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
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

        public ActionResult PayEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PayEmployee(Pagamento pagamento)
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
            return View();
        }

        public ActionResult EmployeePayment()
        {
            // Recupero i dati dalla query string
            int id_Employee = Convert.ToInt32(Request.QueryString["id_Employee"]);

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
    }
}