﻿using Toolkit.Interfaces;
using Microsoft.AspNetCore.Builder;
using Toolkit.MessageBroker.TransactionOutbox;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.TransactionalOutBox;

public static class TransactionalOutBoxFactory
{
    public static ILogable UseTransactionalOutBox<T>(this WebApplicationBuilder builder) where T : TransactionalOutBoxDbContext
    {
        builder.Services.AddScoped<IRegistrationService, RegistrationService<T>>();
        return new TransactionalOutBox<T>(builder);
    }
}