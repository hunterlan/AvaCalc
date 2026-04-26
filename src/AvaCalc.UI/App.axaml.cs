using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaCalc.Core.Shared;
using AvaCalc.UI.Factories;
using AvaCalc.UI.Services;
using AvaCalc.UI.ViewModels;
using AvaCalc.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvaCalc.UI;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _serviceProvider = BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // Mode ViewModel map — add new modes here as they are implemented
        // Key: CalculatorMode enum value, Value: concrete ViewModel type
        var modeViewModelTypes = new Dictionary<CalculatorMode, Type>
        {
            [CalculatorMode.Simple] = typeof(SimpleCalculatorViewModel)
        };

        services.AddSingleton<SimpleCalculatorViewModel>();
        services.AddSingleton<ICalculatorModeViewModelFactory>(sp =>
            new CalculatorModeViewModelFactory(sp, modeViewModelTypes));
        services.AddSingleton<IModeNavigationService>(sp =>
            new ModeNavigationService(
                sp.GetRequiredService<ICalculatorModeViewModelFactory>(),
                CalculatorMode.Simple));
        services.AddSingleton<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }
}