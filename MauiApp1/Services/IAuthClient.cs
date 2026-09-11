using Duende.IdentityModel.OidcClient;
#if WINDOWS
    using MauiApp1.Platforms.Windows;
#endif

namespace MauiApp1.Services;

public interface IAuthClient
{
    public Task<LoginResult> LoginAsync();
}

public class AuthClient : IAuthClient
{
    public async Task<LoginResult> LoginAsync()
    {
#if WINDOWS
        var browser = new LoopbackBrowser(); // listens on a random free loopback port
        var options = new OidcClientOptions
        {
            ClientId = "ID",
            ClientSecret = "SECRET",
            RedirectUri = browser.RedirectUri,           // e.g. http://127.0.0.1:53127/
            Browser = browser,
        };
#else
        var options = new OidcClientOptions
        {
            ClientId = "ID",
            RedirectUri = "com.tauthlight.maui:/callback",
            Browser = new MauiAuthenticatorBrowser(),
        };
#endif
        // Google serves its endpoints from several hosts; OidcClient rejects that by default.
        options.Scope = "openid profile email";
        options.Authority = "https://accounts.google.com";
        options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add("https://oauth2.googleapis.com");
        options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add("https://openidconnect.googleapis.com");
        options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add("https://www.googleapis.com");
        return await new OidcClient(options).LoginAsync(new LoginRequest());
    }
}