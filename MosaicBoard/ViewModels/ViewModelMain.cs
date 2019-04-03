using Microsoft.Win32;
using MosaicBoard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MosaicBoard.ViewModels
{
    public class ViewModelMain : DependencyObject
    {
        public Grid MainGrid { get; set; }

        public ViewModelMain() {
            MainGrid = new Grid
            {
                ShowGridLines = true,
                Background = Brushes.LightGray,
                AllowDrop = true
            };

            for (int i = 0; i < 50; ++i) {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }
        }

        public RelayCommand OpenCommand => new RelayCommand(OpenMosaic);
        public RelayCommand SaveCommand => new RelayCommand(SaveMosaic);
        public RelayCommand CloseCommand => new RelayCommand(()=>Environment.Exit(0));
        public RelayCommand RandomCommand => new RelayCommand(RandomMosaic);
        public RelayCommand ClearCommand => new RelayCommand(ClearMosaic);

        private void RandomMosaic() {
            Random rng = new Random();
            var tiles = Directory.GetFiles("../../Images/Tiles");
            MainGrid.Children.Clear();

            for (int i = 0; i < 50; ++i)
                for(int j = 0; j < 50; ++j)
                {
                    Image rngImage = new Image
                    {
                        Source = new BitmapImage(new Uri(tiles[rng.Next(0, tiles.Length - 1)], UriKind.Relative)),
                        Width = 30,
                        Height = 30
                    };
                    Grid.SetRow(rngImage, i);
                    Grid.SetColumn(rngImage, j);
                    MainGrid.Children.Add(rngImage);
                }
            MainWindow.UpdateGrid();
        }

        private void ClearMosaic() {
            MainGrid.Children.Clear();
            MainWindow.UpdateGrid();
        }

        private void OpenMosaic() {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Mosaic Files (*.mos)|*.mos", InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName };
            MainGrid.Children.Clear();

            // Browse for a .mos mosaic file 
            if (openFileDialog.ShowDialog() == true) {
                string[] tiles = File.ReadAllLines(openFileDialog.FileName);
                foreach (string s in tiles)
                    MainGrid.Children.Add(XamlReader.Parse(s) as Image);
                MainWindow.UpdateGrid();
            }
            else
                return;
        }

        private void SaveMosaic() {
            SaveFileDialog saveFile = new SaveFileDialog { Filter = "Mosaic Files (*.mos)|*.mos", InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName };

            if(saveFile.ShowDialog() == true) {
                MainGrid = MainWindow.MainGrid;

                using (TextWriter tw = new StreamWriter(saveFile.FileName)) {
                    foreach (UIElement tile in MainGrid.Children)
                    if (tile.GetType() == typeof(Image))
                        tw.WriteLine(XamlWriter.Save(tile));
                }

                MessageBox.Show("Mosaic successfully saved.", "Successful Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
