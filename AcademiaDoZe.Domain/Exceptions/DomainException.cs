﻿// Lorena Espeche

namespace AcademiaDoZe.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}