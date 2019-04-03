// Author:  Jordan Travaux
// Date:    April 1, 2019
// File:    MainWindow.xaml.cs
// Purpose: The view of the mosaic board

using MosaicBoard.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MosaicBoard
{
    public partial class MainWindow : Window
    {
        Image piece;
        static ViewModelMain vm;
        public static Grid MainGrid { get; set; }
        public static void UpdateGrid() => MainGrid = vm.Mosaic;

        public MainWindow() {
            InitializeComponent();
            vm = new ViewModelMain();
            DataContext = vm;

            // Add event handlers for drag and drop
            UpdateGrid();
            MainGrid.Drop += Grid_Drop;
            MainGrid.PreviewMouseRightButtonDown += Mosaic_PreviewMouseRightButtonDown;
            canvas.Content = MainGrid;
        }

        private void Grid_Drop(object sender, DragEventArgs e) {
            Image tile = piece;
            Image newPiece = new Image {
                Source = tile.Source,
                Width = 30,
                Height = 30
            };

            // Set the grid row and column that user selected
            Point point = e.GetPosition(MainGrid);
            KeyValuePair<int, int> pos = GetGridSpot(point);
            Grid.SetRow(newPiece, pos.Key);
            Grid.SetColumn(newPiece, pos.Value);
            MainGrid.Children.Add(newPiece);

            piece.Opacity = 1;
        }

        private void DragStarting(object sender, MouseButtonEventArgs e) {
            piece = sender as Image;
            piece.Opacity = 0.2;
            DragDrop.DoDragDrop(sender as DependencyObject, piece, DragDropEffects.Copy);
        }

        private void Mosaic_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            var hitTest = VisualTreeHelper.HitTest(MainGrid, e.GetPosition(MainGrid));
            if(hitTest.VisualHit.GetType() == typeof(Image))
                MainGrid.Children.Remove(hitTest.VisualHit as Image);
        }

        // Purpose: Detirmine what cell the drop was in based on the mouse position
        private KeyValuePair<int, int> GetGridSpot(Point p) {
            int row = 0, col = 0;
            double accumulatedHeight = 0.0, accumulatedWidth = 0.0;

            // calculate the row the mouse was over
            foreach (RowDefinition rowDefinition in MainGrid.RowDefinitions) {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= p.Y)
                    break;
                row++;
            }

            // calculate the column the mouse was over
            foreach (ColumnDefinition columnDefinition in MainGrid.ColumnDefinitions) {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= p.X)
                    break;
                col++;
            }

            return new KeyValuePair<int, int>(row, col);
        }

    }
}
