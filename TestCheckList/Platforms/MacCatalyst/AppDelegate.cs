using System.IO;
using System.Threading.Tasks;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using UIKit;

namespace TestCheckList;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	[Export("application:openURL:options:")]
	public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
	{
		if (url != null && url.IsFileUrl)
		{
			var path = url.Path;
			if (File.Exists(path))
			{
				// Delay slightly to ensure App/VM is ready if cold start
				Task.Run(async () =>
				{
					// Retry a few times if services aren't ready?
					// Usually IPlatformApplication.Current is set by the time this fires if App is launching.
					// But let's be safe.
					var services = IPlatformApplication.Current?.Services;
					if (services == null)
						return;

					var vm = services.GetService<TestCheckList.ViewModels.MainAppViewModel>();
					if (vm != null)
					{
						await vm.LoadFileAsync(path);
					}
				});
				return true;
			}
		}
		return base.OpenUrl(application, url, options);
	}
}
