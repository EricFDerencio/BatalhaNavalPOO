public class Navio
{
    public string Tipo { get; set; } // uma letra P, E, C, D

    public int Tamanho { get; set; }

    public Navio(string tipo)
    {
        Tipo = tipo;
        DeterminarNavio(tipo);
    }

    public int DeterminarNavio(string tipo)
    {
        switch (tipo.ToUpper())
        {
            case "P": //PORTA AVIOES 5 LUGARES
                return Tamanho = 5;

            case "E": //ENCOURAÇADO 4 LUGARES
                return Tamanho = 4;

            case "C": //CRUZADOR 3 LUGARES
                return Tamanho = 3;

            case "D": //DESTROYER 2 LUGARES
                return Tamanho = 2;

            default://DIGITOU ERRADO TAMANHO É ZERO
                return Tamanho = 0;

        }
    }
}

