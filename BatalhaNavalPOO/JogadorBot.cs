using System;
using System.Collections.Generic;

public class JogadorBot : Jogador
{
    private const int MAX_TENTATIVAS_POSICIONAR = 1000;

    private readonly Queue<(int, int)> alvos = new Queue<(int, int)>();
    private readonly HashSet<(int, int)> tentativas = new HashSet<(int, int)>();
    private bool cacando = true; // true = tiro aleatório, false = atacando vizinhos de um acerto

    public JogadorBot(Mapa mapa, List<Navio> navios)
        : base("Bot", mapa, navios) { }

    public override void PosicionarNavios()
    {
        foreach (Navio navio in Navios)
        {
            bool posicionado = false;
            int tentativas = 0;

            do
            {
                if (++tentativas > MAX_TENTATIVAS_POSICIONAR)
                {
                    throw new InvalidOperationException($"O bot não conseguiu posicionar o {NomeNavio(navio)}.");
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
        int tamanho = mapaInimigo.TamanhoMapa;

        if (tentativas.Count >= (tamanho - 1) * (tamanho - 1))
        {
            throw new InvalidOperationException("O bot não tem mais posições para atacar.");
        }

        string resultado = RESULTADO_REPETIDO;

        do
        {
            try
            {
                (int linha, int coluna) = ProximoAlvo(tamanho);

                if (!tentativas.Add((linha, coluna)))
                {
                    continue;
                }

                Console.WriteLine($"O bot atirou na linha {linha}, coluna {coluna}.");

                resultado = Disparar(mapaInimigo, linha, coluna);
                Console.WriteLine(Mensagem(resultado));

                if (resultado != RESULTADO_REPETIDO)
                {
                    AtualizarEstado(resultado, linha, coluna, tamanho);
                }
            }
            catch (CoordenadaInvalidaException)
            {
                resultado = RESULTADO_REPETIDO;
                alvos.Clear();
                cacando = true;
            }
        } while (resultado == RESULTADO_REPETIDO);

        return resultado;
    }

    private (int, int) ProximoAlvo(int tamanho)
    {
        if (!cacando && alvos.Count > 0)
        {
            return alvos.Dequeue();
        }

        cacando = true;
        return (Random.Shared.Next(1, tamanho), Random.Shared.Next(1, tamanho));
    }

    private void AtualizarEstado(string resultado, int linha, int coluna, int tamanho)
    {
        if (resultado == RESULTADO_ACERTOU)
        {
            cacando = false;
            EnfileirarVizinhos(linha, coluna, tamanho);
        }
        else if (resultado == RESULTADO_AFUNDOU)
        {
            alvos.Clear();
            cacando = true;
        }
        else if (alvos.Count == 0)
        {
            cacando = true;
        }
    }

    private void EnfileirarVizinhos(int linha, int coluna, int tamanho)
    {
        List<(int, int)> vizinhos = new List<(int, int)>
        {
            (linha - 1, coluna),
            (linha + 1, coluna),
            (linha, coluna - 1),
            (linha, coluna + 1)
        };

        vizinhos.ForEach(vizinho =>
        {
            bool dentro = vizinho.Item1 >= 1 && vizinho.Item1 < tamanho
                       && vizinho.Item2 >= 1 && vizinho.Item2 < tamanho;

            if (dentro && !tentativas.Contains(vizinho))
            {
                alvos.Enqueue(vizinho);
            }
        });
    }
}