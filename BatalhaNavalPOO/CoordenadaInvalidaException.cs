using System;

public class CoordenadaInvalidaException : Exception
{
    public CoordenadaInvalidaException(string mensagem) : base(mensagem) { }
}