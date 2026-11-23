using System;
using System.Collections.Generic;

public abstract class Jogador
{
    protected const string SIMBOLO_AGUA = "~";
    protected const string SIMBOLO_ACERTO = "X";
    protected const string SIMBOLO_ERRO = "o";

    public const string RESULTADO_AGUA = "Agua";
    public const string RESULTADO_ACERTOU = "Acertou";
    public const string RESULTADO_AFUNDOU = "Afundou";
    public const string RESULTADO_REPETIDO = "Repetido";

    public string Nome { get; }
    public List<Navio> Navios { get; }
    public Mapa Mapa { get; }

    // Perdeu quando nenhum navio da frota resta no próprio tabuleiro
    public bool Perdeu
    {
        get
        {
            foreach (Navio navio in Navios)
            {
                if (ExisteNoTabuleiro(Mapa, navio.Tipo))
                {
                    return false;
                }
            }
            return true;
        }
    }

    protected Jogador(string nome, Mapa mapa, List<Navio> navios)
    {
        HashSet<string> tiposUsados = new HashSet<string>();

        foreach (Navio navio in navios)
        {
            if (navio.Tamanho <= 0)
            {
                throw new ArgumentException($"Tipo de navio inválido: '{navio.Tipo}'.");
            }

            // Um navio de cada tipo, senão não dá para saber quando um afunda
            if (!tiposUsados.Add(navio.Tipo.ToUpper()))
            {
                throw new ArgumentException($"Navio repetido na frota: '{navio.Tipo}'.");
            }
        }

        Nome = nome;
        Mapa = mapa;
        Navios = navios;
    }

    public abstract void PosicionarNavios();
    public abstract string Atacar(Mapa mapaInimigo);

    // Mostra o mapa escondendo os navios que ainda não foram atingidos
    public void MostrarMapaOculto()
    {
        for (int linha = 0; linha < Mapa.TamanhoMapa; linha++)
        {
            for (int coluna = 0; coluna < Mapa.TamanhoMapa; coluna++)
            {
                string celula = Mapa.Tabuleiro[linha, coluna];

                if (linha > 0 && coluna > 0)
                {
                    if (celula == SIMBOLO_ERRO)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (celula == SIMBOLO_ACERTO)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        // Água ou navio intacto: aparece como água
                        Console.ForegroundColor = ConsoleColor.Blue;
                        celula = SIMBOLO_AGUA;
                    }
                }

                Console.Write($"{celula} ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    protected bool PodePosicionar(int linha, int coluna, int tamanho, bool horizontal)
    {
        for (int i = 0; i < tamanho; i++)
        {
            int linhaAtual;
            int colunaAtual;

            if (horizontal)
            {
                linhaAtual = linha;
                colunaAtual = coluna + i;
            }
            else
            {
                linhaAtual = linha + i;
                colunaAtual = coluna;
            }

            if (linhaAtual < 1 || colunaAtual < 1 || linhaAtual >= Mapa.TamanhoMapa || colunaAtual >= Mapa.TamanhoMapa)
            {
                return false;
            }

            if (Mapa.Tabuleiro[linhaAtual, colunaAtual] != SIMBOLO_AGUA)
            {
                return false;
            }
        }

        return true;
    }

    protected void ColocarNavio(Navio navio, int linha, int coluna, bool horizontal)
    {
        if (!PodePosicionar(linha, coluna, navio.Tamanho, horizontal))
        {
            throw new CoordenadaInvalidaException("Posição inválida! O navio sai do campo ou sobrepõe outro. Tente novamente.\n");
        }

        for (int i = 0; i < navio.Tamanho; i++)
        {
            int linhaAtual;
            int colunaAtual;

            if (horizontal)
            {
                linhaAtual = linha;
                colunaAtual = coluna + i;
            }
            else
            {
                linhaAtual = linha + i;
                colunaAtual = coluna;
            }

            Mapa.Tabuleiro[linhaAtual, colunaAtual] = navio.Tipo.ToUpper();
        }
    }

    // Aplica o tiro no mapa do inimigo e devolve o que aconteceu
    protected static string Disparar(Mapa alvo, int linha, int coluna)
    {
        int max = alvo.TamanhoMapa - 1;

        if (linha < 1 || linha > max || coluna < 1 || coluna > max)
        {
            throw new CoordenadaInvalidaException($"Use números de 1 a {max}.\n");
        }

        string celula = alvo.Tabuleiro[linha, coluna];

        if (celula == SIMBOLO_ACERTO || celula == SIMBOLO_ERRO)
        {
            return RESULTADO_REPETIDO;
        }

        if (celula == SIMBOLO_AGUA)
        {
            alvo.Tabuleiro[linha, coluna] = SIMBOLO_ERRO;
            return RESULTADO_AGUA;
        }

        // Era um navio (a célula guarda a letra dele)
        alvo.Tabuleiro[linha, coluna] = SIMBOLO_ACERTO;

        if (ExisteNoTabuleiro(alvo, celula))
        {
            return RESULTADO_ACERTOU;
        }

        return RESULTADO_AFUNDOU;
    }

    protected static string NomeNavio(Navio navio)
    {
        switch (navio.Tipo.ToUpper())
        {
            case "P": return "Porta-aviões";
            case "E": return "Encouraçado";
            case "C": return "Cruzador";
            case "D": return "Destroyer";
            default: return navio.Tipo;
        }
    }

    protected static string Mensagem(string resultado)
    {
        switch (resultado)
        {
            case RESULTADO_AGUA: return "Água!\n";
            case RESULTADO_ACERTOU: return "Acertou!\n";
            case RESULTADO_AFUNDOU: return "Afundou um navio!\n";
            case RESULTADO_REPETIDO: return "Você já atirou nessa posição! Escolha outra.\n";
            default: return "Resultado desconhecido.\n";
        }
    }

    private static bool ExisteNoTabuleiro(Mapa mapa, string letra)
    {
        for (int linha = 1; linha < mapa.TamanhoMapa; linha++)
        {
            for (int coluna = 1; coluna < mapa.TamanhoMapa; coluna++)
            {
                if (mapa.Tabuleiro[linha, coluna] == letra.ToUpper())
                {
                    return true;
                }
            }
        }

        return false;
    }
}