Mapa mapaJogador = new Mapa();
Mapa mapaMaquina = new Mapa();

Mapa tirosJogador = new Mapa();
Mapa tirosMaquina = new Mapa();
string[] navios = { "P", "E", "C", "D" };

int acertosJogador = 0;
int acertosMaquina = 0;

void PosicionarNavios(Mapa mapa)
{
    foreach (string navio in navios)
    {
        Navio navio1 = new Navio(navio);
        bool posicaoValida = false;

        while (!posicaoValida)
        {

            int linha = Random.Shared.Next(1, mapa.TamanhoMapa - 1);
            int coluna = Random.Shared.Next(1, mapa.TamanhoMapa - 1);
            int direcao = Random.Shared.Next(0, 2);//0 horizontal, 1 vertical

            posicaoValida = true;

            if (direcao == 0)
            {
                if (coluna + navio1.Tamanho > mapa.TamanhoMapa)
                    posicaoValida = false;
                else
                {
                    for (int i = 0; i < navio1.Tamanho; i++)
                    {
                        if (mapa.Tabuleiro[linha, coluna + i] != "~")
                            posicaoValida = false;
                    }
                }
            }
            else
            {
                if (linha + navio1.Tamanho > mapa.TamanhoMapa)
                    posicaoValida = false;
                else
                {
                    for (int i = 0; i < navio1.Tamanho; i++)
                    {
                        if (mapa.Tabuleiro[linha + i, coluna] != "~")
                            posicaoValida = false;
                    }
                }
            }

            if (posicaoValida)
            {
                for (int i = 0; i < navio1.Tamanho; i++)
                {
                    if (direcao == 0)
                        mapa.Tabuleiro[linha, coluna + i] = navio1.Tipo;
                    else
                        mapa.Tabuleiro[linha + i, coluna] = navio1.Tipo;
                }
            }
        }
    }
}

void Jogar()
{
    PosicionarNavios(mapaJogador);
    PosicionarNavios(mapaMaquina);
    Console.WriteLine("Mapa do jogador:");
    mapaJogador.MostrarMapa();

    Console.WriteLine("Mapa da maquina:");
    mapaMaquina.MostrarMapa();

    Console.WriteLine("Mapa dos tiros maquina:");
    tirosMaquina.MostrarMapa();
    while (acertosJogador < 14 && acertosMaquina < 14)
    {
        bool jogarNovamente;

        do
        {
            Console.WriteLine("Mapa dos tiros jogador:");
            tirosJogador.MostrarMapa();
            Console.Write($"\nDigite o numero da coluna (1 a {mapaJogador.TamanhoMapa - 1}): ");
            int coluna;
            while (!int.TryParse(Console.ReadLine(), out coluna))
            {
                Console.WriteLine("Valor inválido!");
                Console.WriteLine($"Digite o número da coluna (1 a {mapaJogador.TamanhoMapa - 1}): ");

            }

            Console.Write($"Digite o número da linha (1 a {mapaJogador.TamanhoMapa - 1}): ");
            int linha;
            while(!int.TryParse(Console.ReadLine(),out linha))
            {
                Console.WriteLine("Valor inválido!");
                Console.WriteLine($"Digite o número da linha (1 a {mapaJogador.TamanhoMapa - 1}): ");

            }

            while (linha < 0 || linha >= mapaJogador.TamanhoMapa  || coluna < 0 || coluna >= mapaJogador.TamanhoMapa)
            {

                Console.WriteLine("Coordenada inválida!");

                Console.Write($"\nDigite o numero da coluna (1 a {mapaJogador.TamanhoMapa - 1}): ");
                
                while (!int.TryParse(Console.ReadLine(), out coluna))
                {
                    Console.WriteLine("Valor inválido!");
                    Console.WriteLine($"Digite o número da coluna (1 a {mapaJogador.TamanhoMapa - 1}): ");

                }

                Console.Write($"Digite o número da linha (1 a {mapaJogador.TamanhoMapa - 1}): ");
                
                while (!int.TryParse(Console.ReadLine(), out linha))
                {
                    Console.WriteLine("Valor inválido!");
                    Console.Write($"Digite o número da linha (1 a {mapaJogador.TamanhoMapa - 1}): ");

                }

                jogarNovamente = false;
                
            }

            if (tirosJogador.Tabuleiro[linha, coluna] != "~")
            {
                Console.WriteLine("Você já atacou essa posição!");
                Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();

                jogarNovamente = false;
                continue;
            }

            if (mapaMaquina.Tabuleiro[linha, coluna] != "~")
            {
                tirosJogador.Tabuleiro[linha, coluna] = "X";
                acertosJogador++;

                Console.WriteLine("\nVocê acertou!");
                jogarNovamente = true;
            }
            else
            {
                tirosJogador.Tabuleiro[linha, coluna] = "O";

                Console.WriteLine("\nVocê errou!");
                jogarNovamente = false;
            }
            Console.WriteLine();
            Console.WriteLine("Mapa dos tiros jogador:");
            tirosJogador.MostrarMapa();
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();

        } while (jogarNovamente && acertosJogador < 14);

        if (acertosJogador == 14)
            break;

        do
        {
            Console.Clear();

            int linha = Random.Shared.Next(1, mapaMaquina.TamanhoMapa - 1);
            int coluna = Random.Shared.Next(1, mapaMaquina.TamanhoMapa - 1);

            while (tirosMaquina.Tabuleiro[linha, coluna] != "~")
            {
                linha = Random.Shared.Next(1, mapaMaquina.TamanhoMapa - 1);
                coluna = Random.Shared.Next(1, mapaMaquina.TamanhoMapa - 1);
            }

            Console.WriteLine($"\nA máquina atacou Linha: {linha}, Coluna : {coluna}");

            if (mapaJogador.Tabuleiro[linha, coluna] != "~")
            {
                tirosMaquina.Tabuleiro[linha, coluna] = "X";
                acertosMaquina++;

                Console.WriteLine("A máquina acertou seu navio!");
                jogarNovamente = true;
            }
            else
            {
                tirosMaquina.Tabuleiro[linha, coluna] = "O";

                Console.WriteLine("A máquina acertou a água!");
                jogarNovamente = false;
            }

            tirosMaquina.MostrarMapa();

            Console.WriteLine("Mapa do jogador:");

            mapaJogador.MostrarMapa();

            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();

        } while (jogarNovamente && acertosMaquina < 14);
    }

    Console.Clear();

    mapaJogador.MostrarMapa();
    tirosJogador.MostrarMapa();

    if (acertosJogador == 14)
        Console.WriteLine("\nPARABÉNS! VOCÊ VENCEU!");
    else
        Console.WriteLine("\nA MÁQUINA VENCEU!");
}

Jogar();

