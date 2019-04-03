// Author:  Jordan Travaux
// Date:    April 1, 2019
// File:    App.xaml.cs
// Purpose: The main startup of the mosaic board

using System.Windows;

namespace MosaicBoard {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) => new MainWindow().Show();
    }
}
