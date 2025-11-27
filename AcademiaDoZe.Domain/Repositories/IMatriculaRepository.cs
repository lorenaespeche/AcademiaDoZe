// Lorena Espeche

using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

public interface IMatriculaRepository : IRepository<Matricula>
{
    Task<Matricula> ObterPorAluno(int alunoId);
    Task<IEnumerable<Matricula>> ObterAtivas(int alunoId = 0);
    Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias);
    Task<Matricula> ObterPorAlunoCpf(string cpf);
}