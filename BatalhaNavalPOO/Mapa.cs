public class Mapa
{
    public string[,] Tabuleiro { get; set; }

    public int TamanhoMapa { get; private set; } = 10; //tamanho da matriz quadrada


    public Mapa() //CONSTRUTOR PARA CRIAR E PREENCHER O MAPA BASE
    {
        Tabuleiro = new string[TamanhoMapa, TamanhoMapa];
        PrencherMapa();
    }

    private void PrencherMapa()
    {
        for (int i = 0; i < TamanhoMapa; i++)
        {
            for (int j = 0; j < TamanhoMapa; j++)
            {
                if (i == 0) Tabuleiro[i, j] = j.ToString(); //preenchendo a primeira linha com indices
                else if (j == 0) Tabuleiro[i, j] = i.ToString(); //preenchendo a primeira coluna com indices
                else Tabuleiro[i, j] = "~"; // PREENCHENDO O MAPA COM ~ PARA SER A AGUA
            }
        }
    }

    public void MostrarMapa()
    {
        for (int i = 0; i < TamanhoMapa; i++)
        {
            for (int j = 0; j < TamanhoMapa; j++)
            {
                Console.Write($"{Tabuleiro[i, j]} "); //imprimindo o mapa
            }
            Console.WriteLine(""); //pulando linha
        }
    }


}

