//{**
// This code block adds the code to launch the rate and review dialog from the settings page
//**}
namespace Param_ItemNamespace.Views
{
    public sealed partial class Param_SettingsPageNamePage : Page, INotifyPropertyChanged
    {
        //^^
        //{[{

        private async void RateReviewLink_Click(object sender, RoutedEventArgs e)
        {
            // For more details see https://docs.microsoft.com/en-us/windows/uwp/monetize/request-ratings-and-reviews
            var result = await StoreRequestHelper.SendRequestAsync(StoreContext.GetDefault(), 16, String.Empty);

            // TODO WTS: Handle the response from showing the dialog (optional)
            if (result.ExtendedError == null)
            {
                JObject jsonObject = JObject.Parse(result.Response);

                if (jsonObject.SelectToken("status").ToString() == "success")
                {
                    // The customer rated or reviewed the app.
                    // See the above link for more details about possible server responses.
                }
            }
        }
        //}]}
    }
}
