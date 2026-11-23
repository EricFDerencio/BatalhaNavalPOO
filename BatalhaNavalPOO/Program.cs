List<Navio> navios = new List<Navio> { new Navio("P"), new Navio("E"), new Navio("C"), new Navio("D") };

Console.WriteLine("Digite o nome do Jogador:");

JogadorPessoa jogador = new JogadorPessoa(Console.ReadLine(), new Mapa(), navios);
JogadorBot bot = new JogadorBot(new Mapa(), navios);

string validar;
int auxiliar;

do
{
    Console.WriteLine("Voce quer posicionar a frota?(S/N)");
    validar = Console.ReadLine().ToUpper();
    auxiliar = 0;

    switch (validar)
    {
        case "S":
            jogador.PosicionarNavios();
            bot.PosicionarNavios();
            auxiliar = 0;
            break;

        case "N":
            jogador.PosicionarNaviosAleatorio();
            bot.PosicionarNavios();
            auxiliar = 0;
            break;

        default:
            Console.WriteLine("Opção inválida!");
            auxiliar = 1;
            break;
    }
} while (auxiliar != 0);

do
{
    Console.WriteLine("===== MAPA DO BOT =====");
    bot.MostrarMapaOculto();

    Console.WriteLine("===== SEU MAPA =====");
    jogador.Mapa.MostrarMapa();

    jogador.Atacar(bot.Mapa);

    if (!bot.Perdeu)
    {
        bot.Atacar(jogador.Mapa);
    }
} while (!jogador.Perdeu && !bot.Perdeu);


if (!bot.Perdeu)
{

    Console.WriteLine("Parabéns, você venceu!");

}
else

{

    Console.WriteLine("O bot venceu!");

}