using MosaicBoard.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MosaicBoard
{
    public partial class MainWindow : Window
    {
        Image piece;

        public MainWindow() {
            InitializeComponent();
            DataContext = new ViewModelMain();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            Image toolboxPiece = piece;
            int row, col;

            Image newPiece = new Image {
                Source = toolboxPiece.Source,
                Width = 30,
                Height = 30,
                IsHitTestVisible = true,
            };

            Point p = e.GetPosition(mosaic);

            var s = VisualTreeHelper.HitTest(mosaic, p);
            if(s.VisualHit.GetType() == typeof(Canvas))
            {
                mosaic.Children.Add(newPiece);
                Canvas.SetLeft(newPiece, p.X);
                Canvas.SetTop(newPiece, p.Y);
                //Grid.SetRow(newPiece, row);
                //Grid.SetColumn(newPiece, col);
            }



            piece.Opacity = 1;
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;

        }

        private void DragStarting(object sender, MouseButtonEventArgs e) {
            piece = sender as Image;
            piece.Opacity = 0.2;

            DragDrop.DoDragDrop((DependencyObject)sender, piece, DragDropEffects.Copy);
        }
    }
}
