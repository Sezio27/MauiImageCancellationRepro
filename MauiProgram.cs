using Microsoft.Extensions.Logging;

namespace MauiImageCancellationRepro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		builder.Logging.SetMinimumLevel(LogLevel.Warning);
		builder.Logging.AddDebug();

		return builder.Build();
	}
}
