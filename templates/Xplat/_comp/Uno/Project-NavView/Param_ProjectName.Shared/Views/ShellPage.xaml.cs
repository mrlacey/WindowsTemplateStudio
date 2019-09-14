using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;

            // Handle back button
            this.ContentFrame.Navigated += (s, a) => { this.navigationView.IsBackEnabled = this.ContentFrame.CanGoBack; };
            this.navigationView.BackRequested += (s, a) => { if (this.ContentFrame.CanGoBack) this.ContentFrame.GoBack(); };

            this.ContentFrame.Navigate(typeof(Param_HomeNamePage), null, null);
        }

        public void OnSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (((FrameworkElement)args.SelectedItem).Tag.ToString())
            {
            }
        }
    }
}
