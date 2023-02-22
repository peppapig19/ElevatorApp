using System;
using System.Windows;

namespace ElevatorApp
{
    public partial class FloorCount : Window
    {
        public FloorCount()
        {
            InitializeComponent();
        }

        public int Count => Convert.ToInt32(count_slider.Value);

        private void count_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (count_label != null) count_label.Content = count_slider.Value.ToString();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}