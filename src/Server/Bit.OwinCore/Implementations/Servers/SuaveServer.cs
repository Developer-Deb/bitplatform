﻿using Bit.OwinCore.Implementations.Servers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bit.OwinCore.Implementations.Servers
{
    public class SuaveServer : OwinServer, IServer
    {
        private IDisposable _suaveServer;

        public void Start<TContext>(IHttpApplication<TContext> application)
        {
            CreateOwinProps(application, out Func<IDictionary<string, object>, Task> appFunc, out Dictionary<string, object> props);

            Suave.Owin.OwinServerFactory.Initialize(props);

            _suaveServer = Suave.Owin.OwinServerFactory.Create(appFunc, props);
        }

        public void Dispose()
        {
            _suaveServer?.Dispose();
        }
    }
}

namespace Microsoft.AspNetCore.Hosting
{
    public static class SuaveWebHostBuilderExtensions
    {
        public static IWebHostBuilder UseSuave(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(services =>
            {
                services.AddSingleton<IServer, SuaveServer>();
            });
        }
    }
}