using Data;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scan
{
    public static class Initializer
    {
        public static IServiceProvider Initialize()
        {
            return new ServiceCollection()
                .AddDbContext<CoreContext>()
                .AddScoped<IReaderFile, ReaderFile>()
                .AddScoped<IParserFile, ParserFile>()
                .AddScoped<IHandler, Handler>()
                .BuildServiceProvider();
        }
    }
}
