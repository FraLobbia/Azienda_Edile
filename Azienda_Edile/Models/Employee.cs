using System.ComponentModel.DataAnnotations;

namespace Azienda_Edile.Models
{
    public class Employee
    {
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
        public Employee(string nome, string cognome, string indirizzo, string codiceFiscale, bool coniugato, int numeroFigliACarico, string mansione)
        {
            Nome = nome;
            Cognome = cognome;
            Indirizzo = indirizzo;
            CodiceFiscale = codiceFiscale;
            Coniugato = coniugato;
            NumeroFigliACarico = numeroFigliACarico;
            Mansione = mansione;
        }
    }
}