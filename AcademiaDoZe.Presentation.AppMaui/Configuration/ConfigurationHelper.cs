﻿using AcademiaDoZe.Application.DependencyInjection;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Presentation.AppMaui.Message;
using CommunityToolkit.Mvvm.Messaging;

namespace AcademiaDoZe.Presentation.AppMaui.Configuration;

/* ConfigurationHelper - config inicial a partir das Preferences - recarga automática via Messenger */
public static class ConfigurationHelper
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // lê as preferências do banco de dados
        var (connectionString, databaseType) = ReadDbPreferences();
        var repoConfig = new RepositoryConfig { ConnectionString = connectionString, DatabaseType = databaseType };
        services.AddSingleton(repoConfig);
        services.AddApplicationServices();

        // assina a mensagem e aplica as mudanças automática
        WeakReferenceMessenger.Default.Register<RepositoryConfig, BancoPreferencesUpdatedMessage>(

        // recipient é o RepositoryConfig singleton
        recipient: repoConfig, handler: static (recipient, message) =>
        {
            // aplica as novas configurações
            var (connectionString, databaseType) = ReadDbPreferences();
            recipient.ConnectionString = connectionString;
            recipient.DatabaseType = databaseType;
        });
    }

    private static (string ConnectionString, EAppDatabaseType DatabaseType) ReadDbPreferences()
    {
        // dados conexão
        string dbServer = Preferences.Get("Servidor", "localhost"); // "172.24.32.1"
        string dbDatabase = Preferences.Get("Banco", "db_academia_do_ze"); // "db_academia_do_ze"
        string dbUser = Preferences.Get("Usuario", "root"); // "sa"
        string dbPassword = Preferences.Get("Senha", "root"); // "abcBolinhas12345"
        // string dbComplemento = Preferences.Get("Complemento", ""); // "TrustServerCertificate=True;Encrypt=True;"
                                                                                                            // Configurações de conexão

        string connectionString = $"Server={dbServer};Database={dbDatabase};User Id={dbUser};Password={dbPassword}";

        // obtém o tipo de banco de dados selecionado nas preferências
        var dbType = Preferences.Get("DatabaseType", EAppDatabaseType.MySql.ToString()) switch
        {
            "SqlServer" => EAppDatabaseType.SqlServer,
            "MySql" => EAppDatabaseType.MySql,
            _ => EAppDatabaseType.MySql

        };
        return (connectionString, dbType);
    }
}