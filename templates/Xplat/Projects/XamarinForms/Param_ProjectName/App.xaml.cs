using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Param_RootNamespace.Views;

namespace Param_RootNamespace
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Param_HomeNamePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
