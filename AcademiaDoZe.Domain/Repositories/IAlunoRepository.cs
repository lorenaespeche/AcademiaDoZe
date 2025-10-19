// Lorena Espeche

using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

public interface IAlunoRepository : IRepository<Aluno>
{
    // nova versão, retornando múltiplos colaboradores
    Task<IEnumerable<Aluno>> ObterPorCpf(string cpf);
    // Task<Aluno?> ObterPorCpf(string cpf);
    Task<bool> CpfJaExiste(string cpf, int? id = null);
    Task<bool> TrocarSenha(int id, string novaSenha);
}