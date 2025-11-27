using AcademiaDoZe.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe.Application.Services
{
    public class CalculoService
    {
        public static int CalcularIdade(DateOnly dataNascimento)
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            int idade = hoje.Year - dataNascimento.Year;

            if (dataNascimento > hoje.AddYears(-idade))
            {
                idade--; // ainda não fez aniversário este ano
            }

            return idade;
        }
    }
}
