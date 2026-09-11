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
            ClientId = "551874123815-h8qjk337l25vdpn8si7v3uto7gmcksf1.apps.googleusercontent.com",
            ClientSecret = "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", // see Windows gotchas
            RedirectUri = browser.RedirectUri,           // e.g. http://127.0.0.1:53127/
            Browser = browser,
        };
#else
        var options = new OidcClientOptions
        {
            ClientId = "551874123815-e6n6sf7ad2676uvqs2rpvglpuqgjaci9.apps.googleusercontent.com",
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