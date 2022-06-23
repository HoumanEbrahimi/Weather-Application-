using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using Weather;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;

namespace Weather

{

    public partial class MainWindow : Window
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
                textSearch.Visibility = Visibility.Collapsed;
            else
                textSearch.Visibility = Visibility.Visible;
        }

        string apiKey = "2ea3790bda9676de193a19d568e3c12e";


        void weatherInfo()
        {
            AllocConsole();

            using (WebClient client = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", txtSearch.Text, apiKey);
                var json=client.DownloadString(url);
                weatherInfo.root Info = JsonConvert.DeserializeObject<weatherInfo.root>(json);

                WindSpeed.Text = Info.wind.speed.ToString();

                int result = 0;
                Console.WriteLine(int.Parse(Info.main.temp.ToString()));

                if (int.TryParse(Info.main.temp.ToString(), out result))
                {

                    int val = (int.Parse(Info.main.temp.ToString()) - 32) * 9 / 5;

                    Texttemp.Text = val.ToString() + "°c";

                }
                CountryName.Text=txtSearch.Text+","+Info.sys.country.ToString();

                

            }




        }
        private void click(object sender, RoutedEventArgs e)
        {

            weatherInfo();

        }
    }
}