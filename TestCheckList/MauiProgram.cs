using Microsoft.Extensions.Logging;
using TestCheckList.Interfaces;
using TestCheckList.ViewModels;
using TestCheckList.Views;

namespace TestCheckList;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Services
		builder.Services.AddSingleton<IMainAppViewModel, MainAppViewModel>();
		builder.Services.AddSingleton<MainAppViewModel>(); // Concrete type for App injection

		// Views
		builder.Services.AddSingleton<MainAppPage>();

		return builder.Build();
	}
}
