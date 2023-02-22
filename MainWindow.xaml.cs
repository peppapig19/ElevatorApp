using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ElevatorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Elevator lift;
        readonly BitmapImage open = new BitmapImage(new Uri("pack://application:,,,/images/open.jpg", UriKind.Absolute));
        readonly BitmapImage closed = new BitmapImage(new Uri("pack://application:,,,/images/closed.jpg", UriKind.Absolute));

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FloorCount dialog = new FloorCount();
            if (dialog.ShowDialog() != true) Close();

            lift = new Elevator(dialog.Count);

            for (int i = 0; i < dialog.Count; i++)
            {
                Button btn = new Button();

                btn.Name = $"floor_{i + 1}";
                btn.Content = $"{i + 1} этаж";
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.Click += floor_button_Click;

                floors_stackPanel.Children.Add(btn);
            }

            lift.PropertyChanged += (obj, args) => doors_image.Source = lift.Doors == Elevator.STAND.Open ? open : closed;
        }

        async void floor_button_Click(object sender, RoutedEventArgs e)
        {
            int n = Convert.ToInt32(((Button)sender).Name.Replace("floor_", string.Empty));
            if (lift.Doors == Elevator.STAND.Open || lift.Cur == n) return;

            Random ran = new Random();
            int m = 0;
            do m = ran.Next(1, lift.count + 1); while (m == lift.Cur);
            pick_label.Content = m.ToString();
            pick_stackPanel.Visibility = Visibility.Visible;

            Progress<string> pr = new Progress<string>(x => current_label.Content = x);

            await lift.PickAsync(n, m, pr);

            if (lift.Cur == n || lift.Cur == m) pick_stackPanel.Visibility = Visibility.Hidden;
        }

        private void open_button_Click(object sender, RoutedEventArgs e) => lift.Open();

        private void close_button_Click(object sender, RoutedEventArgs e) => lift.Close();
    }
}