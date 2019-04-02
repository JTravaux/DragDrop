﻿using MosaicBoard.ViewModels;
using System;
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

        public MainWindow() {
            InitializeComponent();
            DataContext = new ViewModelMain();

            mosaic.ShowGridLines = true;
            for (int i = 0; i < 50; ++i) {
                mosaic.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(30)});
                mosaic.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(30)});
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e) {
            Image toolboxPiece = piece;
            Image newPiece = new Image {
                Source = toolboxPiece.Source,
                Width = 30,
                Height = 30
            };

            // Find the row and column
            Point point = e.GetPosition(mosaic);
            KeyValuePair<int, int> pos = GetGridSpot(point);
            Grid.SetRow(newPiece, pos.Key);
            Grid.SetColumn(newPiece, pos.Value);
            mosaic.Children.Add(newPiece);

            piece.Opacity = 1;
        }

        private void DragStarting(object sender, MouseButtonEventArgs e) {
            piece = sender as Image;
            piece.Opacity = 0.2;
            DragDrop.DoDragDrop(sender as DependencyObject, piece, DragDropEffects.Copy);
        }

        private void Mosaic_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            var hitTest = VisualTreeHelper.HitTest(mosaic, e.GetPosition(mosaic));
            if(hitTest.VisualHit.GetType() == typeof(Image))
                mosaic.Children.Remove(hitTest.VisualHit as Image);
        }

        private KeyValuePair<int, int> GetGridSpot(Point p)
        {
            int row = 0, col = 0;
            double accumulatedHeight = 0.0, accumulatedWidth = 0.0;

            // calculate the row the mouse was over
            foreach (RowDefinition rowDefinition in mosaic.RowDefinitions) {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= p.Y)
                    break;
                row++;
            }

            // calculate the column the mouse was over
            foreach (ColumnDefinition columnDefinition in mosaic.ColumnDefinitions) {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= p.X)
                    break;
                col++;
            }

            return new KeyValuePair<int, int>(row, col);
        }

    }
}
