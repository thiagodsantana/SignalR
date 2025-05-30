using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

Console.WriteLine("Iniciando conexão com SignalR como ADMIN - Console");

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5076/emprestimohub?grupo=admin")
    .WithAutomaticReconnect()
    .Build();

connection.On<object>("NovaSolicitacao", emp =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Nova Solicitação: {JsonSerializer.Serialize(emp)}");
    Console.ResetColor();
});

connection.On<object>("StatusAtualizado", emp =>
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"Status Atualizado: {JsonSerializer.Serialize(emp)}");
    Console.ResetColor();
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Conectado com sucesso como ADMIN");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Erro ao conectar: {ex.Message}");
    Console.ResetColor();
    return;
}

Console.WriteLine("Aguardando eventos... Pressione Ctrl+C para sair.");
await Task.Delay(Timeout.Infinite);
