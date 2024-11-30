using System.Configuration;
using System.Data;
using System.Windows;

namespace SchwabCSVReader;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Attach to parent console (if any)
        ConsoleHelper.AttachToParentConsole();
        
    }
}