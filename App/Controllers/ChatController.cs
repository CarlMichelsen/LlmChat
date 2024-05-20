using System.Text.Json;
using Domain.Dto.Chat;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ChatController(
    ILogger<ChatController> logger) : ControllerBase
{
    [HttpPost]
    public async Task SendMessage([FromBody] NewUserMessageDto newUserMessageDto)
    {
        logger.LogInformation("SendMessage: {UserMessageJson}", JsonSerializer.Serialize(newUserMessageDto));

        var directory = AppContext.BaseDirectory;
        var path = Path.Combine(directory, "MockData/mock-response.txt");

        Console.WriteLine("START");
        await foreach (var line in System.IO.File.ReadLinesAsync(path))
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            await this.HttpContext.Response.WriteAsync(line + '\n');
            Console.WriteLine(line);
        }
        
        Console.WriteLine("END");
    }
}
