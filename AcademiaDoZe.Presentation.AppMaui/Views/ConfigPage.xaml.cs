﻿using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Presentation.AppMaui.Message;
using CommunityToolkit.Mvvm.Messaging;

namespace AcademiaDoZe.Presentation.AppMaui.Views;

public partial class ConfigPage : ContentPage
{
    // ordem de foco dos controles usada por OnEntryCompleted
    private VisualElement[] _focusOrder = [];

    public ConfigPage()
    {
        InitializeComponent();

        CarregarTema();
        CarregarBanco();

        // Assina o evento SelectedIndexChanged do TemaPicker
        // Utilizando o tratador OnSalvarTemaClicked já existente
        TemaPicker.SelectedIndexChanged += OnSalvarTemaClicked;

        // inicializa a ordem de foco dos controles
        _focusOrder = [
        DatabaseTypePicker, ServidorEntry, BancoEntry, UsuarioEntry, SenhaEntry, ComplementoEntry ];
    }

    private void CarregarTema()
    {
        // uso de expressão switch para carregar o índice selecionado
        TemaPicker.SelectedIndex = Preferences.Get("Tema", "system") switch { "light" => 0, "dark" => 1, _ => 2, };
    }
    
    private async void OnSalvarTemaClicked(object sender, EventArgs e)
    {
        string selectedTheme = TemaPicker.SelectedIndex switch { 0 => "light", 1 => "dark", _ => "system" };
        Preferences.Set("Tema", selectedTheme);
        // disparar mensagem para uso na recarga dinâmica
        WeakReferenceMessenger.Default.Send(new TemaPreferencesUpdatedMessage("TemaAlterado"));
        await DisplayAlert("Sucesso", "Dados salvos com sucesso!", "OK");
        // navegar para dashboard
        await Shell.Current.GoToAsync("//dashboard");
    }
    
    // banco de Dados
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // retornar para dashboard
        await Shell.Current.GoToAsync("//dashboard");
    }
    
    // ao fechar a página, chama WeakReferenceMessenger.Default.UnregisterAll(this); para evitar vazamentos de memória - memory leaks
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // desinscreve o mensageiro para evitar memory leaks
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    // banco de Dados
    private void CarregarBanco()
    {
        foreach (var tipo in Enum.GetValues<EAppDatabaseType>())
        {
            DatabaseTypePicker.Items.Add(tipo.ToString());
        }
        // carregar os dados existentes, ou valores padrão, ao abrir a página
        ServidorEntry.Text = Preferences.Get("Servidor", "");
        BancoEntry.Text = Preferences.Get("Banco", "");
        UsuarioEntry.Text = Preferences.Get("Usuario", "");
        SenhaEntry.Text = Preferences.Get("Senha", "");
        //ComplementoEntry.Text = Preferences.Get("Complemento", "TrustServerCertificate=True;Encrypt=True;");
        DatabaseTypePicker.SelectedItem = Preferences.Get("DatabaseType", EAppDatabaseType.MySql.ToString());
    }
    
    private async void OnSalvarBdClicked(object sender, EventArgs e)
    {
        Preferences.Set("Servidor", ServidorEntry.Text);
        Preferences.Set("Banco", BancoEntry.Text);
        Preferences.Set("Usuario", UsuarioEntry.Text);
        Preferences.Set("Senha", SenhaEntry.Text);
       // Preferences.Set("Complemento", ComplementoEntry.Text);
        Preferences.Set("DatabaseType", DatabaseTypePicker.SelectedItem.ToString());
        // disparar a mensagem para recarga dinâmica
        WeakReferenceMessenger.Default.Send(new BancoPreferencesUpdatedMessage("BancoAlterado"));
        await DisplayAlert("Sucesso", "Dados salvos com sucesso!", "OK");
        // navegar para dashboard
        await Shell.Current.GoToAsync("//dashboard");
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        if (sender is not VisualElement current)
            return;
        int idx = Array.IndexOf(_focusOrder, current);
        if (idx >= 0)
        {
            if (idx < _focusOrder.Length - 1)
            {
                // foca o próximo controle
                _focusOrder[idx + 1].Focus();
            }
            else
            {
                // último item -> submete
                OnSalvarBdClicked(sender, e);
            }
        }
        else
        {
            // fallback simples: avançar para o primeiro focável se não estiver na lista
            _focusOrder.FirstOrDefault()?.Focus();
        }
    }
}