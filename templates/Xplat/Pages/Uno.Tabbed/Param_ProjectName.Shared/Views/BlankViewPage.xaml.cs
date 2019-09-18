using System;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class BlankViewPage : Page
    {
        private BlankViewViewModel ViewModel;

        public BlankViewPage()
        {
            InitializeComponent();

            this.ViewModel = new BlankViewViewModel();
        }
    }
}
