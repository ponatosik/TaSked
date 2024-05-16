using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Plugin.Firebase.Android;
#endif

namespace TaSked.App.Common.Notifications;

public static class FirebaseSettings
{
	public static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
	{
		builder.ConfigureLifecycleEvents(events =>
		{
#if ANDROID
			events.AddAndroid(android => android.OnCreate((activity, state) =>
				CrossFirebase.Initialize(activity, null, CreateCrossFirebaseSettings())));
#endif
		});

		builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
		return builder;
	}

	public static CrossFirebaseSettings CreateCrossFirebaseSettings()
	{
		return new CrossFirebaseSettings(isAuthEnabled: true, isCloudMessagingEnabled: true);
	}
}
