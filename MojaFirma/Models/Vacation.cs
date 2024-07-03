using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MojaFirma.Models
{
    public class Vacation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa urlopu jest wymagana.")]
        public string VacationName { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data zakończenia jest wymagana.")]
        public DateTime EndDate { get; set; }

        public string Comment { get; set; }

        public string UserId { get; set; } // Klucz obcy użytkownika

        public IdentityUser User { get; set; } // Powiązanie z użytkownikiem
    }
}
