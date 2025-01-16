using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.Widget;
using CommunityToolkit.Mvvm.Messaging;

namespace TaSked.App
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    
    [IntentFilter(new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.ActionView,
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataScheme = "http", DataHost = "tasked.com", DataPathPrefix = "/group/join"

        )
    ]

    
    [IntentFilter(new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.ActionView,
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataScheme = "https", DataHost = "tasked.com", DataPathPrefix = "/group/join"

        )
    ]
    
	public class MainActivity : MauiAppCompatActivity
	{
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            var uri = Intent?.Data;

            if (uri != null)
            {
                string invintationId = uri.ToString().Substring(uri.ToString().LastIndexOf('=') + 1);
                WeakReferenceMessenger.Default.Send(new InvintationItemMessage(invintationId));
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var action = intent.Action;
            var strLink = intent.DataString;
            
            if (Intent.ActionView == action && !string.IsNullOrWhiteSpace(strLink))
            {
                Toast.MakeText(this, strLink, ToastLength.Short).Show();
                string invintationId = strLink.Substring(strLink.LastIndexOf('=') + 1);
                WeakReferenceMessenger.Default.Send(new InvintationItemMessage(invintationId));
            }
        }
    }
}
