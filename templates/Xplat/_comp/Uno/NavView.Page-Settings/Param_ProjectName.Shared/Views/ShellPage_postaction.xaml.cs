namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
        public void OnSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs args)
        {
//{[{
            if (args.IsSettingsSelected)
            {
                this.ContentFrame.Navigate(typeof(SettingsPage), null, null);
                return;
            }

//}]}
        }
    }
}
