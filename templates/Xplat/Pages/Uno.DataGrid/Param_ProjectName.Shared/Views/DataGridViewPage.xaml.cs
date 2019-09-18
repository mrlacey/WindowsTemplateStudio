using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    public sealed partial class DataGridViewPage : Page
    {
        private DataGridViewViewModel ViewModel = new DataGridViewViewModel();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DataGridViewPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public DataGridViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
