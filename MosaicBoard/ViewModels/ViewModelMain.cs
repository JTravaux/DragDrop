// Author:  Jordan Travaux
// Date:    April 1, 2019
// File:    ViewModelMain.cs
// Purpose: The ViewModel of the mosaic board

using Microsoft.Win32;
using MosaicBoard.Helpers;
using MosaicBoard.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MosaicBoard.ViewModels
{
    public class ViewModelMain : DependencyObject
    {
        public Grid Mosaic { get; set; }

        public ViewModelMain() {
            MosaicModel m = new MosaicModel();
            Mosaic = m.MainGrid;
        }

        public RelayCommand OpenCommand => new RelayCommand(OpenMosaic);
        public RelayCommand SaveCommand => new RelayCommand(SaveMosaic);
        public RelayCommand RandomCommand => new RelayCommand(RandomMosaic);
        public RelayCommand ClearCommand => new RelayCommand(ClearMosaic);
        public RelayCommand CloseCommand => new RelayCommand(() => Environment.Exit(0));
        public RelayCommand AboutCommand => new RelayCommand(() => MessageBox.Show("Developed by: Jordan Travaux\n" + "\nInstructions...\nDrag a tile from the toolbox onto the grid.\nTo remove a tile, right click on it.", "About Mosaic Board", MessageBoxButton.OK, MessageBoxImage.Information));
        public RelayCommand GridLineCommand => new RelayCommand(() => { Mosaic.ShowGridLines = !Mosaic.ShowGridLines; MainWindow.UpdateGrid(); });

        private void RandomMosaic() {
            Mosaic.Children.Clear();
            Random rng = new Random();
            string[] tiles = Directory.GetFiles("../../Images/Tiles");
            List<string> strs = new List<string>();

            // Make the full squares more likely (looks a bit nicer)
            for(int i = 0; i < tiles.Length; ++i) {
                strs.Add(tiles[i]);
                if (tiles[i].EndsWith("1.png") || tiles[i].EndsWith("2.png") || tiles[i].EndsWith("3.png") || tiles[i].EndsWith("4.png") || tiles[i].EndsWith("5.png") || tiles[i].EndsWith("6.png")) {
                    strs.Add(tiles[i]);
                    strs.Add(tiles[i]);
                }
            }

            for (int i = 0; i < 50; ++i)
                for(int j = 0; j < 50; ++j) {
                    Image rngImage = new Image {
                        Source = new BitmapImage(new Uri(strs[rng.Next(0, strs.Count-1)], UriKind.Relative)),
                        Width = 30,
                        Height = 30
                    };
                    Grid.SetRow(rngImage, i);
                    Grid.SetColumn(rngImage, j);
                    Mosaic.Children.Add(rngImage);
                }
            MainWindow.UpdateGrid();
        }

        private void ClearMosaic() {
            Mosaic.Children.Clear();
            MainWindow.UpdateGrid();
        }

        private void OpenMosaic() {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Mosaic Files (*.mos)|*.mos", InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName };
            Mosaic.Children.Clear();

            // Browse for a .mos mosaic file 
            if (openFileDialog.ShowDialog() == true) {
                string[] tiles = File.ReadAllLines(openFileDialog.FileName);
                foreach (string s in tiles)
                    Mosaic.Children.Add(XamlReader.Parse(s) as Image);
                MainWindow.UpdateGrid();
            }
            else
                return;
        }

        private void SaveMosaic() {
            SaveFileDialog saveFile = new SaveFileDialog { Filter = "Mosaic Files (*.mos)|*.mos", InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName };

            if(saveFile.ShowDialog() == true) {
                Mosaic = MainWindow.MainGrid;

                using (TextWriter tw = new StreamWriter(saveFile.FileName)) {
                    foreach (UIElement tile in Mosaic.Children)
                    if (tile.GetType() == typeof(Image))
                        tw.WriteLine(XamlWriter.Save(tile));
                }

                MessageBox.Show("Mosaic successfully saved.", "Successful Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
