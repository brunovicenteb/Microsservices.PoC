﻿using MassTransit;
using Toolkit.TransactionalOutBox;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.OutBox.Consumer;

internal class ConsumerOutBoxStarter<T> : OutBoxStarter where T : OutBoxDbContext, new()
{
    internal ConsumerOutBoxStarter(WebApplicationBuilder builder, string dbTypeVarName, string dbConnectionVariableName)
        : base(builder, dbTypeVarName, dbConnectionVariableName)
    {
    }

    protected override string TelemetryName => "consumer";

    protected override void DoUseDatabase(string stringConnection)
    {
        Builder.Services.AddDbContext<T>(o =>
        {
            if (OutBoxDbContext.DbType == DatabaseType.SqlServer)
                UseSqlServer(stringConnection, o);
            else
                UsePostgress(stringConnection, o);
        });
    }

    protected override void DoUseRabbitMq(string host)
    {
        Builder.Services.AddMassTransit(busRegistration =>
        {
            busRegistration.AddEntityFrameworkOutbox<T>(o =>
            {
                if (OutBoxDbContext.DbType == DatabaseType.SqlServer)
                    o.UseSqlServer();
                else
                    o.UsePostgres();
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });
            busRegistration.SetKebabCaseEndpointNameFormatter();
            var context = new T();
            context.RegisterConsumers(Builder.Services, busRegistration);
            busRegistration.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
                cfg.ConfigureEndpoints(context);
            });
        });
    }

    //public override IDatabaseable UseOpenTelemetry()
    //{
    //    Builder.Services.AddOpenTelemetryTracing(builder =>
    //    {
    //        builder.SetResourceBuilder(ResourceBuilder.CreateDefault()
    //                .AddService("consumer")
    //                .AddTelemetrySdk()
    //                .AddEnvironmentVariableDetector())
    //            .AddSource("MassTransit")
    //            .AddJaegerExporter(o =>
    //            {
    //                o.AgentHost = HostMetadataCache.IsRunningInContainer ? "jaeger" : "localhost";
    //                o.AgentPort = 6831;
    //                o.MaxPayloadSizeInBytes = 4096;
    //                o.ExportProcessorType = ExportProcessorType.Batch;
    //                o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
    //                {
    //                    MaxQueueSize = 2048,
    //                    ScheduledDelayMilliseconds = 5000,
    //                    ExporterTimeoutMilliseconds = 30000,
    //                    MaxExportBatchSize = 512,
    //                };
    //            });
    //    });
    //    return this;
    //}

    private void UsePostgress(string stringConnection, DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(stringConnection, options =>
        {
            options.MinBatchSize(1);
        });
    }

    private void UseSqlServer(string stringConnection, DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(stringConnection, options =>
        {
            options.MinBatchSize(1);
        });
    }
}