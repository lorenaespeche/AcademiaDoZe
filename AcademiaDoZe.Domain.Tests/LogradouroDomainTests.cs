// Lorena Espeche

using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests;

public class LogradouroDomainTests
{
    [Fact]
    public void CriarLogradouro_Valido_NaoDeveLancarExcecao()
    {
        var logradouro = Logradouro.Criar(2, "12345670", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        Assert.NotNull(logradouro); // validando cria��o, n�o deve lan�ar exce��o e n�o deve ser nulo
    }

    [Fact]
    public void CriarLogradouro_Invalido_DeveLancarExcecao()
    {
        // validando a cria��o de logradouro com CEP inv�lido, deve lan�ar exce��o
        Assert.Throws<DomainException>(() => Logradouro.Criar(5, "123", "Rua A", "Centro", "Cidade", "SP", "Brasil"));
    }

    [Fact]
    public void CriarLogradouro_Valido_VerificarNormalizado()
    {
        var logradouro = Logradouro.Criar(3, "12.3456-78 ", " Rua A ", " Centro ", " Cidade ", "S P", "Brasil ");
        Assert.Equal("12345678", logradouro.Cep); // validando normaliza��o
        Assert.Equal("Rua A", logradouro.Nome);
        Assert.Equal("Centro", logradouro.Bairro);
        Assert.Equal("Cidade", logradouro.Cidade);
        Assert.Equal("SP", logradouro.Estado);
        Assert.Equal("Brasil", logradouro.Pais);

    }

    [Fact]
    public void CriarLogradouro_Invalido_VerificarMessageExcecao()
    {
        var exception = Assert.Throws<DomainException>(() => Logradouro.Criar(4, "12345670", "", "Centro", "Cidade", "SP", "Brasil"));
        Assert.Equal("NOME_OBRIGATORIO", exception.Message); // validando a mensagem de exce��o
    }
}