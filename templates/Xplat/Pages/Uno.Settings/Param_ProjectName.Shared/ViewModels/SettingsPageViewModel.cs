using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Param_RootNamespace.Helpers;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Param_RootNamespace.ViewModels
{
    // TODO WTS: Add settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsPageViewModel : BaseViewModel
    {
        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }

        public SettingsPageViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
