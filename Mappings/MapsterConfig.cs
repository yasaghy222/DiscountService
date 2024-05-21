﻿using System.Reflection;
using Mapster;
namespace DiscountService.Mappings;

public static class MapsterConfig
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
	}
}
