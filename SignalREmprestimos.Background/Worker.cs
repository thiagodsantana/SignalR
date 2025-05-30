using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace SignalREmprestimos.Background
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        private readonly HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:5076/emprestimohub?grupo=admin").WithAutomaticReconnect().Build();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {

                    Console.WriteLine("Iniciando conexão com SignalR como ADMIN - Background Service");
                    connection.On<object>("NovaSolicitacao", emp =>
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Nova Solicitação: {JsonSerializer.Serialize(emp)}");
                        Console.ResetColor();
                    });

                    connection.On<object>("StatusAtualizado", emp =>
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Status Atualizado: {JsonSerializer.Serialize(emp)}");
                        Console.ResetColor();
                    });

                    try
                    {
                        await connection.StartAsync(stoppingToken);
                        Console.WriteLine("Conectado com sucesso como ADMIN");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Erro ao conectar: {ex.Message}");
                        Console.ResetColor();
                        return;
                    }

                    Console.WriteLine("Aguardando eventos." +
                        ".. Pressione Ctrl+C para sair.");
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
            }
        }
    }
}
