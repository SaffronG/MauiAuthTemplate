[https://claude.ai/code/artifact/aaee3136-069b-4f81-ad91-d72e9d9ebcd8]

Drop in code for your login & logout commands

```
        public Command LoginCommand => field ??= new(async () =>
        {
            AuthLoginResult loginResult = await _authClient.LoginAsync();
            if (!loginResult.IsError)
            {
                Username = loginResult?.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Failed to get username";
                UserPicture = loginResult?.User?.Claims.FirstOrDefault(c => c.Type == "picture")?.Value ?? "Failed to get profile picture";

                (IsLoginViewVisible, IsHomeViewVisible) = (false, true);
            }
            else
            {
                await _shellClient.DisplayAlertAsync("Error", loginResult?.ErrorDescription, "OK");
            }
        });
        public Command LogoutCommand => field ??= new(async () =>
        {
            await _authClient.LogoutAsync();
            (IsLoginViewVisible, IsHomeViewVisible) = (true, false);
        });
```

Required Libraries
```
        Duende.IdentityModel.OidcClient
```

Add this for Android
```
        <queries>
            <intent>
			<action android:name="android.support.customtabs.action.CustomTabsService" />
	    </intent>
	</queries>
```

```
using Android.App;
using Android.Content.PM;

namespace MauiApp1.Platforms.Android;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(
              [Intent.ActionView],
              Categories = new[] {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
              },
              DataScheme = CALLBACK_SCHEME,
              DataHost = "com.test.auth0")]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "myapp";
}
```

Add this for Windows
```
namespace MauiApp1.Platforms.Windows;

using Duende.IdentityModel.OidcClient.Browser;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class LoopbackBrowser : IBrowser
{
    private readonly TcpListener _listener = new(IPAddress.Loopback, 0); // port 0 = OS picks a free one

    public string RedirectUri { get; }

    public LoopbackBrowser()
    {
        _listener.Start();
        RedirectUri = $"http://127.0.0.1:{((IPEndPoint)_listener.LocalEndpoint).Port}/";
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken ct = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromMinutes(5));

        try
        {
            await Launcher.Default.OpenAsync(new Uri(options.StartUrl));

            while (true)
            {
                using var client = await _listener.AcceptTcpClientAsync(cts.Token);
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, leaveOpen: true);

                // First line of the HTTP request, e.g. "GET /?state=...&code=... HTTP/1.1"
                var target = (await reader.ReadLineAsync(cts.Token))?.Split(' ').ElementAtOrDefault(1);
                var isCallback = target is not null && (target.Contains("code=") || target.Contains("error="));

                await RespondAsync(stream,
                    isCallback ? "Sign-in complete. You can close this tab and return to the app."
                               : "Waiting for sign-in...",
                    cts.Token);

                if (isCallback)
                {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.Success,
                        Response = RedirectUri.TrimEnd('/') + target,
                    };
                }
                // Anything else (favicon, speculative connection) is ignored; keep waiting.
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return new BrowserResult { ResultType = BrowserResultType.UserCancel };
        }
        catch (OperationCanceledException)
        {
            return new BrowserResult { ResultType = BrowserResultType.Timeout };
        }
        finally
        {
            _listener.Stop();
        }
    }

    private static async Task RespondAsync(Stream stream, string message, CancellationToken ct)
    {
        var body = Encoding.UTF8.GetBytes(
            $"<html><body style=\"font-family:sans-serif;padding:2rem\"><h3>{message}</h3></body></html>");
        var head = Encoding.ASCII.GetBytes(
            $"HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n");

        await stream.WriteAsync(head, ct);
        await stream.WriteAsync(body, ct);
    }
}
```

May need if Authority is declined
```
		---------------------------------------IAuthClient.cs--------------------------------------------
        options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add("https://oauth2.googleapis.com");
        options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add("https://openidconnect.googleapis.com");
        options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add("https://www.googleapis.com");
```
