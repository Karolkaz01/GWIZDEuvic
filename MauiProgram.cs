using System.Reflection;

using Gwizd.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gwizd;

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
        builder.RegisterAppConfiguration();

        builder.RegisterServices();

        return builder.Build();
	}
}
