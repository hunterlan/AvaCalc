using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaCalc.UI.ViewModels;
using AvaCalc.UI.Views;

namespace AvaCalc.UI;

/// <summary>
/// Given a view model, returns the corresponding view.
/// Uses an explicit compile-time mapping to remain Native AOT and trim-safe —
/// no reflection, no <c>Type.GetType</c>, no <c>Activator.CreateInstance</c>.
/// Add a new entry to <see cref="_viewMap"/> for every new ViewModel/View pair.
/// </summary>
public sealed class ViewLocator : IDataTemplate
{
    // Keyed by the concrete ViewModel type; value is a factory that creates the matching View.
    // Every entry is resolved at compile time, making this fully AOT-compatible.
    private static readonly Dictionary<Type, Func<Control>> _viewMap = new()
    {
        [typeof(SimpleCalculatorViewModel)] = () => new SimpleCalculatorView()
    };

    /// <inheritdoc/>
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        if (_viewMap.TryGetValue(param.GetType(), out var factory))
            return factory();

        return new TextBlock { Text = $"Not Found: {param.GetType().FullName}" };
    }

    /// <inheritdoc/>
    public bool Match(object? data) => data is ViewModelBase;
}
