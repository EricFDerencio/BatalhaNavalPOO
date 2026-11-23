using System;
using System.Collections.Generic;

public class JogadorPessoa : Jogador
{
    private const int MAX_TENTATIVAS_POSICIONAR = 1000;
    public JogadorPessoa(string nome, Mapa mapa, List<Navio> navios)
        : base(nome, mapa, navios) { }

    public override void PosicionarNavios()
    {
        int max = Mapa.TamanhoMapa - 1;

        foreach (Navio navio in Navios)
        {
            bool posicionado = false;

            do
            {
                try
                {
                    Mapa.MostrarMapa();
                    Console.WriteLine($"{Nome}, posicione o {NomeNavio(navio)} (tamanho {navio.Tamanho})");

                    int linha = LerNumero($"Linha (1-{max})", 1, max);
                    int coluna = LerNumero($"Coluna (1-{max})", 1, max);
                    int direcao = LerNumero("Direção (1 - Horizontal, 2 - Vertical)", 1, 2);

                    bool horizontal = direcao == 1;

                    ColocarNavio(navio, linha, coluna, horizontal);
                    posicionado = true;
                }
                catch (CoordenadaInvalidaException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (!posicionado);
        }
    }
    public  void PosicionarNaviosAleatorio()
    {
        foreach (Navio navio in Navios)
        {
            bool posicionado = false;
            int tentativas = 0;

            do
            {
                if (++tentativas > MAX_TENTATIVAS_POSICIONAR)
                {
                    throw new InvalidOperationException($"Não foi possível posicionar o {NomeNavio(navio)}.");
                }

                try
                {
                    int linha = Random.Shared.Next(1, Mapa.TamanhoMapa);
                    int coluna = Random.Shared.Next(1, Mapa.TamanhoMapa);
                    bool horizontal = Random.Shared.Next(2) == 0;

                    ColocarNavio(navio, linha, coluna, horizontal);
                    posicionado = true;
                }
                catch (CoordenadaInvalidaException)
                {
                    // Posição ruim, sorteia outra
                }
            } while (!posicionado);
        }
    }
    public override string Atacar(Mapa mapaInimigo)
    {
        int max = mapaInimigo.TamanhoMapa - 1;
        string resultado = RESULTADO_REPETIDO;

        do
        {
            try
            {
                int linha = LerNumero($"Linha do tiro (1-{max})", 1, max);
                int coluna = LerNumero($"Coluna do tiro (1-{max})", 1, max);

                resultado = Disparar(mapaInimigo, linha, coluna);
                Console.WriteLine(Mensagem(resultado));
            }
            catch (CoordenadaInvalidaException ex)
            {
                resultado = RESULTADO_REPETIDO;
                Console.WriteLine(ex.Message);
            }
        } while (resultado == RESULTADO_REPETIDO);

        return resultado;
    }

    private static int LerNumero(string mensagem, int minimo, int maximo)
    {
        int numero;

        do
        {
            Console.Write($"{mensagem}: ");
        } while (!int.TryParse(Console.ReadLine(), out numero) || numero < minimo || numero > maximo);

        return numero;
    }
}