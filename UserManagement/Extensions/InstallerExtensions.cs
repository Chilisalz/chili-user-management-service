﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UserManagementService.Installers;

namespace UserManagementService.Extensions
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration Configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, Configuration));
        }
    }
}
