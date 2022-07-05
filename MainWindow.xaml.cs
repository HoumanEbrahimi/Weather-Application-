using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using Weather;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media.Imaging;
using weatherForecast;

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

        string unit="metric";

        private void Celsius(object sender, RoutedEventArgs e)
        {
            unit = "metric";

        }

        private void Fahrenheit(object sender, RoutedEventArgs e)
        {

        }

        void weatherInfo(object sender,RoutedEventArgs e)
        {

            using (WebClient client = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid={2}", txtSearch.Text,unit, apiKey);

                var json =client.DownloadString(url);
                weatherInfo.root Info = JsonConvert.DeserializeObject<weatherInfo.root>(json);

                WindSpeed.Text = Info.wind.speed.ToString();

                Texttemp.Text = Info.main.temp.ToString() + "°c";
                CountryName.Text=txtSearch.Text+","+Info.sys.country.ToString();
                WeatherCondition.Text = "Feels like :"+Info.main.feels_like.ToString()+ " °c";
                HumidityContext.Text = Info.main.humidity.ToString();

                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

                System.DateTime dtDateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);


                dtDateTime = dtDateTime.AddSeconds(int.Parse(Info.sys.sunrise.ToString())).ToLocalTime();

                dtDateTime2 = dtDateTime2.AddSeconds(int.Parse(Info.sys.sunset.ToString())).ToLocalTime();

                SunriseContext.Text = dtDateTime.ToString();

                SunsetContext.Text = dtDateTime2.ToString();

                if (int.Parse(Info.main.humidity.ToString())>30 && int.Parse(Info.main.humidity.ToString()) < 50)
                {
                    humidContext.Text = "normal";
                    howHumid.Source = new BitmapImage(new Uri(@"/Images/happy.png", UriKind.Relative));
                }

                else if (int.Parse(Info.main.humidity.ToString()) > 50)
                {
                    humidContext.Text = "High humidity";
                    howHumid.Source = new BitmapImage(new Uri(@"/Images/dislike.png", UriKind.Relative));

                }

                else if (int.Parse(Info.main.humidity.ToString()) < 30)
                {
                    humidContext.Text = "Not humid";
                    howHumid.Source = new BitmapImage(new Uri(@"/Images/like.png", UriKind.Relative));

                }


                if (Info.weather[0].description.ToString() == "clear sky") 
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/sun.png", UriKind.Relative));

                } 

                else if (Info.weather[0].description.ToString() == "broken clouds" || Info.weather[0].description.ToString() == "overcast clouds")
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/cloud.png", UriKind.Relative));
                }

                else if(Info.weather[0].description.ToString() =="shower rain")
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/rain.png", UriKind.Relative));

                }

                else if(Info.weather[0].description.ToString() == "rain")
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/rain_cloud.png", UriKind.Relative));

                }


                else if (Info.weather[0].description.ToString() == "snow")
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/snow.png", UriKind.Relative));

                }

                else if (Info.weather[0].description.ToString() == "thunderstorm")
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/storm.png", UriKind.Relative));

                }

                else if(Info.weather[0].description.ToString() == "mist")
                {
                    imageCondition.Source = new BitmapImage(new Uri(@"/Images/water.png", UriKind.Relative));

                }
                if (int.Parse(Info.wind.deg.ToString()) > 0 && int.Parse(Info.wind.deg.ToString()) < 90)
                {
                    windDirection.Text = "NE";
                }

                else if (int.Parse(Info.wind.deg.ToString()) == 0 || int.Parse(Info.wind.deg.ToString()) == 360) { windDirection.Text = "E"; }

                else if(int.Parse(Info.wind.deg.ToString()) == 90) { windDirection.Text = "N"; }
                else if (int.Parse(Info.wind.deg.ToString()) > 90 && int.Parse(Info.wind.deg.ToString()) < 135)
                {
                    windDirection.Text = "NW";
                }
                else if(int.Parse(Info.wind.deg.ToString())>135 && int.Parse(Info.wind.deg.ToString())<180){ windDirection.Text = "WN"; }
                else if(int.Parse(Info.wind.deg.ToString()) == 180) { windDirection.Text = "W"; }
                else if (int.Parse(Info.wind.deg.ToString())>180 && int.Parse(Info.wind.deg.ToString()) < 225) { windDirection.Text = "SW"; }
                else if(int.Parse(Info.wind.deg.ToString())>225 && int.Parse(Info.wind.deg.ToString()) <270) { windDirection.Text = "WS"; }
                else if (int.Parse(Info.wind.deg.ToString()) == 270) { windDirection.Text = "S"; }
                else if(int.Parse(Info.wind.deg.ToString()) >270 && int.Parse(Info.wind.deg.ToString())<360) { windDirection.Text = "SE"; }

                string uvIndexURL = string.Format("http://api.openweathermap.org/data/2.5/air_pollution?lat={0}&lon={1}&appid={2}", Info.coord.lat, Info.coord.lon, apiKey);
                var json2 = client.DownloadString(uvIndexURL);
                weatherInfo.root Infos = JsonConvert.DeserializeObject<weatherInfo.root>(json2);

                AllocConsole();
                Console.WriteLine((Info.coord.lat, Info.coord.lon));
                UvVal.Text = Infos.list[0].main.aqi.ToString();

                if (Infos.list[0].main.aqi==1)
                {
                    airQuality.Text = "Good";
                    airQualityCondition.Source = new BitmapImage(new Uri(@"/Images/like.png", UriKind.Relative));

                }

                else if(Infos.list[0].main.aqi==2)
                {
                    airQuality.Text = "Fair";
                    airQualityCondition.Source = new BitmapImage(new Uri(@"/Images/happy.png", UriKind.Relative));


                }

                else if(Infos.list[0].main.aqi==3)
                {
                    airQuality.Text = "Moderate";
                    airQualityCondition.Source = new BitmapImage(new Uri(@"/Images/moderate.png", UriKind.Relative));


                }

                else if(Infos.list[0].main.aqi==4)
                {
                    airQuality.Text = "Poor";
                    airQualityCondition.Source = new BitmapImage(new Uri(@"/Images/dislike.png", UriKind.Relative));

                }
                else
                {
                    airQuality.Text = "Very Poor";
                    airQualityCondition.Source = new BitmapImage(new Uri(@"/Images/dislike.png", UriKind.Relative));


                }
            }

        }

        void getUV(object sender,RoutedEventArgs e)
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&units={1}&appid={2}", txtSearch.Text, unit, apiKey);

                var json = web.DownloadString(url);
                weatherForecast.forecast info2 = JsonConvert.DeserializeObject<weatherForecast.forecast>(json);

          //      tempnext.Text = info2.list[1].main.temp.ToString();


            }

        }
        void getForecast(object sender, RoutedEventArgs e)
        {
          
           // string url = "";
            using(WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&units={1}&appid={2}", txtSearch.Text, unit, apiKey);

                var json =web.DownloadString(url);
                weatherForecast.forecast info2 = JsonConvert.DeserializeObject<weatherForecast.forecast>(json);

                tempnext.Text = info2.list[1].main.temp.ToString() + "°c \n fl:" + info2.list[1].main.feels_like.ToString()+ "°c";

                secondHour.Text= info2.list[2].main.temp.ToString() + "°c\n fl:" + info2.list[2].main.feels_like.ToString() + "°c";

                thirdHour.Text= info2.list[3].main.temp.ToString() + "°c\n fl:" + info2.list[3].main.feels_like.ToString() + "°c";

                fourthHour.Text = info2.list[4].main.temp.ToString() + "°c\n fl:" + info2.list[4].main.feels_like.ToString() + "°c";

                sixthHour.Text= info2.list[5].main.temp.ToString() + "°c\n fl:" + info2.list[5].main.feels_like.ToString() + "°c";
            }
        }
        private void click(object sender, RoutedEventArgs e)
        {

            weatherInfo(sender,e);
            getForecast(sender,e);
        }




    }
}
