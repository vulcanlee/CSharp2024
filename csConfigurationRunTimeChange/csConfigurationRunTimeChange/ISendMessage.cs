
namespace csConfigurationRunTimeChange;

public interface ISendMessage
{
    Task SendAsync(string message);
}