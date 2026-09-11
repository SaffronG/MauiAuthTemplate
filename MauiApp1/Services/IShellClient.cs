namespace MauiApp1.Services;

public interface IShellClient
{
    public Task DisplayAlertAsync(string msg, string desc, string cancel);
}

public class ShellClient : IShellClient
{
    public async Task DisplayAlertAsync(string msg, string desc, string cancel) => await DisplayAlertAsync(msg, desc, cancel);
}