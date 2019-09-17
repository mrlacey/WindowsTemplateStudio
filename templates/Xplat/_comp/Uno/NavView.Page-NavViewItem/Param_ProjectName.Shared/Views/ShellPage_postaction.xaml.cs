
namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
        public void OnSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch (((FrameworkElement)args.SelectedItem).Tag.ToString())
            {
//^^
//{[{
                case "wts.ItemName":
                    this.ContentFrame.Navigate(typeof(wts.ItemNamePage), null, null);
                    break;
//}]}
            }
        }
    }
}
