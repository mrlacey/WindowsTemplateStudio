
namespace Param_RootNamespace
{
    sealed partial class App : Application
    {
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
//{[{
                    rootFrame.Navigate(typeof(ShellPage), e.Arguments);
//}]}
                }
            }
        }
    }
}
