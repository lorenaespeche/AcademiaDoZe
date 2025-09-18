﻿// Lorena Espeche

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Application.Mappings;

public static class MatriculaMappings
{
    public static MatriculaDTO ToDto(this Matricula matricula)
    {
        return new MatriculaDTO
        {
            Id = matricula.Id,
            AlunoMatricula = matricula.AlunoMatricula.ToDto(),
            Plano = matricula.Plano.ToApp(),
            DataInicio = matricula.DataInicio,
            DataFim = matricula.DataFim,
            Objetivo = matricula.Objetivo,
            RestricoesMedicas = matricula.RestricoesMedicas.ToApp(),
            ObservacoesRestricoes = matricula.ObservacoesRestricoes,
            LaudoMedico = matricula.LaudoMedico != null ? new ArquivoDTO { Conteudo = matricula.LaudoMedico.Conteudo } : null, // mapeia laudo para DTO
        };
    }

    public static Matricula ToEntity(this MatriculaDTO matriculaDto)
    {
        return Matricula.Criar(
        matriculaDto.Id,
        matriculaDto.AlunoMatricula.ToEntityMatricula(), // mapeia aluno do DTO para a entidade, resolvendo o caso da senha null
        matriculaDto.Plano.ToDomain(),
        matriculaDto.DataInicio,
        matriculaDto.DataFim,
        matriculaDto.Objetivo,
        matriculaDto.RestricoesMedicas.ToDomain(),
        (matriculaDto.LaudoMedico?.Conteudo != null) ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo) : null!, // mapeia laudo do DTO para a entidade
        matriculaDto.ObservacoesRestricoes!
        );
    }

    public static Matricula UpdateFromDto(this Matricula matricula, MatriculaDTO matriculaDto)
    {
        return Matricula.Criar(
        matricula.Id, // mantém o ID original
        matriculaDto.AlunoMatricula.ToEntityMatricula() ?? matricula.AlunoMatricula,
        matriculaDto.Plano != default ? matriculaDto.Plano.ToDomain() : matricula.Plano,
        matriculaDto.DataInicio != default ? matriculaDto.DataInicio : matricula.DataInicio,
        matriculaDto.DataFim != default ? matriculaDto.DataFim : matricula.DataFim,
        matriculaDto.Objetivo ?? matricula.Objetivo,
        matriculaDto.RestricoesMedicas != default ? matriculaDto.RestricoesMedicas.ToDomain() : matricula.RestricoesMedicas,
        (matriculaDto.LaudoMedico?.Conteudo != null) ? Arquivo.Criar(matriculaDto.LaudoMedico.Conteudo) : matricula.LaudoMedico, // atualiza laudo se fornecido
        matriculaDto.ObservacoesRestricoes ?? matricula.ObservacoesRestricoes
        );
    }
}