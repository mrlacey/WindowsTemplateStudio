using System;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class WebViewPagePage : Page
    {
        private WebViewPageViewModel ViewModel = new WebViewPageViewModel();

        public WebViewPagePage()
        {
            InitializeComponent();
        }
    }
}
