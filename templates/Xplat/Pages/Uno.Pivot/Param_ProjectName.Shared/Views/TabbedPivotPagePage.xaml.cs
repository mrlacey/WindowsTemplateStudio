using System;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class TabbedPivotPagePage : Page
    {
        TabbedPivotPageViewModel ViewModel = new TabbedPivotPageViewModel();
    
        public TabbedPivotPagePage()
        {
            InitializeComponent();
        }
    }
}
