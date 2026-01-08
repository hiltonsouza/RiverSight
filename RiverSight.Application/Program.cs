// Garantir que esse caminho exista e tenha imagens!
using RiverSight.Infrastructure.Vision.Services;
using System.Net.Http.Headers;

string assetsPath = @"C:\RiverSight\Assets";

Console.WriteLine("=== RiverSight Vision Test ===");
Console.WriteLine($"=== Carregando templates de: {assetsPath}");

// Instancia o nosso leitor de tela
using var screenReader = new OpenCvScreenReader(assetsPath);

Console.WriteLine("Iniciando loop de detecção. Pressionce Ctrl+C para sair");

while (true)
{
    Console.Clear();
    Console.WriteLine("Lendo tela...");

    // Pede para a Infra ler as cartas do Herói
    var cards = screenReader.ReadHeroHands();

    if (cards.Any())
    {
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var card in cards)
        {
            //Deve imprimir "Ah", "Ks", etc.
            Console.WriteLine($"[DETECTADO] Carta: {card}");
        }
    }
    else
    {
        Console.ForegroundColor= ConsoleColor.Red;
        Console.WriteLine("[vazio] Nenhuma carta identificada nas coordenadas.");
    }

    Console.ResetColor();
    Thread.Sleep(500); // 2 FPS é suficiente para teste

}
