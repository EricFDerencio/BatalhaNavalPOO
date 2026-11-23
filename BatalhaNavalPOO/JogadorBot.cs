using System;
using System.Collections.Generic;

public class JogadorBot : Jogador
{
    private const int MAX_TENTATIVAS_POSICIONAR = 1000;

    private readonly Queue<Posicao> alvos = new Queue<Posicao>();
    private readonly bool[,] jaAtacou; // true = o bot já atirou nessa posição
    private int totalAtaques = 0;
    private bool cacando = true; // true = tiro aleatório, false = atacando vizinhos de um acerto

    public JogadorBot(Mapa mapa, List<Navio> navios)
        : base("Bot", mapa, navios)
    {
        jaAtacou = new bool[mapa.TamanhoMapa, mapa.TamanhoMapa];
    }

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

        if (totalAtaques >= (tamanho - 1) * (tamanho - 1))
        {
            throw new InvalidOperationException("O bot não tem mais posições para atacar.");
        }

        string resultado = RESULTADO_REPETIDO;

        do
        {
            try
            {
                Posicao alvo = ProximoAlvo(tamanho);

                if (jaAtacou[alvo.Linha, alvo.Coluna])
                {
                    continue;
                }

                jaAtacou[alvo.Linha, alvo.Coluna] = true;
                totalAtaques++;

                Console.WriteLine($"O bot atirou na linha {alvo.Linha}, coluna {alvo.Coluna}.");

                resultado = Disparar(mapaInimigo, alvo.Linha, alvo.Coluna);
                Console.WriteLine(Mensagem(resultado));

                if (resultado != RESULTADO_REPETIDO)
                {
                    AtualizarEstado(resultado, alvo.Linha, alvo.Coluna, tamanho);
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

    private Posicao ProximoAlvo(int tamanho)
    {
        if (!cacando && alvos.Count > 0)
        {
            return alvos.Dequeue();
        }

        cacando = true;
        return new Posicao(Random.Shared.Next(1, tamanho), Random.Shared.Next(1, tamanho));
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
        List<Posicao> vizinhos = new List<Posicao>();
        vizinhos.Add(new Posicao(linha - 1, coluna));
        vizinhos.Add(new Posicao(linha + 1, coluna));
        vizinhos.Add(new Posicao(linha, coluna - 1));
        vizinhos.Add(new Posicao(linha, coluna + 1));

        foreach (Posicao vizinho in vizinhos)
        {
            bool dentro = vizinho.Linha >= 1 && vizinho.Linha < tamanho
                       && vizinho.Coluna >= 1 && vizinho.Coluna < tamanho;

            if (dentro && !jaAtacou[vizinho.Linha, vizinho.Coluna])
            {
                alvos.Enqueue(vizinho);
            }
        }
    }
}