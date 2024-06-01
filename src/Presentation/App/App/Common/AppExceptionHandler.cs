using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ReactiveUI;
using Refit;
using System.Diagnostics;
using System.Net;
using System.Reactive.Concurrency;

namespace TaSked.App.Common;

public class AppExceptionHandler : IObserver<Exception>
{
	public void OnNext(Exception error)
	{
		//if (Debugger.IsAttached) Debugger.Break();

		if (error is ApiException || error is HttpRequestException || error is WebException)
		{
			string text = $"Error: {error.Message}";
			ToastDuration duration = ToastDuration.Long;
			double fontSize = 16;
			var toast = Toast.Make(text, duration, fontSize);

			RxApp.MainThreadScheduler.Schedule(() => toast.Show());
		}
		else
		{
			RxApp.MainThreadScheduler.Schedule(() => throw error);
		}
	}

	public void OnError(Exception error)
	{
		if (Debugger.IsAttached) Debugger.Break();

		if (error is ApiException || error is HttpRequestException)
		{
			string text = $"Error: {error.Message}";
			ToastDuration duration = ToastDuration.Long;
			double fontSize = 16;
			var toast = Toast.Make(text, duration, fontSize);

			RxApp.MainThreadScheduler.Schedule(() => toast.Show());
		}
		else
		{
			RxApp.MainThreadScheduler.Schedule(() => throw error);
		}
	}

	public void OnCompleted()
	{
		if (Debugger.IsAttached) Debugger.Break();
	}
}