using MauiApp1.Services;
using System.ComponentModel;

namespace MauiApp1.ViewModels;

public partial class MainPageViewModel(IShellClient shellClient, IAuthClient client) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly IShellClient _shellClient = shellClient;
    private readonly IAuthClient _authClient = client;
    public bool IsLoginViewVisible
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoginViewVisible)));
        }
    } = true;
    public bool IsHomeViewVisible
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsHomeViewVisible)));
        }
    } = false;
    public string Username
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
        }
    } = String.Empty;
    public string UserPicture
    {
        get;
        set
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserPicture)));
        }
    } = String.Empty;

    public Command LoginCommand => field ??= new(async () =>
    {
        var result = await _authClient.LoginAsync();
        if (!result.IsError)
        {
            Username = result.User.FindFirst("name")?.Value ?? "Unknown";
            UserPicture = result.User.FindFirst("picture")?.Value ?? "";
            (IsLoginViewVisible, IsHomeViewVisible) = (false, true);
        }
        else
        {
            await _shellClient.DisplayAlertAsync("Error", result.ErrorDescription, "OK");
        }
    });
    public Command LogoutCommand => field ??= new(async () =>
    {
        await _authClient.LoginAsync();
        (IsLoginViewVisible, IsHomeViewVisible) = (true, false);
    });
}
