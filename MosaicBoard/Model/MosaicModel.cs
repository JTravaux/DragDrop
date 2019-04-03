// Author:  Jordan Travaux
// Date:    April 2, 2019
// File:    MosaicModel.cs
// Purpose: The Model of the mosaic board

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MosaicBoard.Model
{
    public class MosaicModel
    {
        public Grid MainGrid { get; set; }
        public MosaicModel() {
            MainGrid = new Grid {
                ShowGridLines = true,
                Background = Brushes.LightGray,
                AllowDrop = true
            };

            for (int i = 0; i < 50; ++i) {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }
        }
    }
}
