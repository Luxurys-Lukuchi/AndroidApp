using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AndroidClass
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Обернуть MainPage в NavigationPage
            MainPage = new NavigationPage(new MainPage());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
