using Android.App;
using Android.Content;
using Android.Content.PM;

namespace MauiApp1.Platforms.Android;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(
              [Intent.ActionView],
              Categories = new[] {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
              },
              DataScheme = CALLBACK_SCHEME)]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "com.tauthlight.maui";
}
