using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using ModernSoapApp.Helper.Entities;
using ModernSoapApp.Service;
using ModernSoapApp.Views;

namespace ModernSoapApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
          #region Class Level Members

        private NetworkStatusService _networkStatusService;
        private ConfigurationService _configurationService;
        public Configuration _configuration;
        private string _accessToken = string.Empty;
        private static string _strItemClicked;
        private bool DoSync = false;

        // TODO may be in future if necessary means this data will also from server
        // Currently using static fields as menu items for displaying
        private string[] _strMenuItems = new string[] { "Leads", "Opportunities", "Accounts", "Contacts", "Tasks", "Placeholder", "Placeholder" };
        private ObservableCollection<MainPageItem> _theMenuItems { get; set; }

        # endregion
     
        public MainMenu()
        {
            this.InitializeComponent();
        }



       

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _networkStatusService = new NetworkStatusService();
            _configurationService = new ConfigurationService();

            if (e.Parameter is Configuration)
            {
                _configuration = (Configuration) e.Parameter;
                DoSync = false;
            }
            else
            {
                ////////////// Toto sa spusta iba ak ides prvy krat na Main view
                DoSync = true;
            }
            progressBar.Visibility = Visibility.Visible;
            Initialize();
        }

        /// <summary>      
        /// Binding Menu items to Main Page
        /// </summary>
        private async void Initialize()
        {

            progressBar.Visibility = Visibility.Collapsed;
            
            await _configurationService.SaveConfiguration();
           _configuration=await _configurationService.RestoreConfiguration();
            if (_configuration == null)
            {
                MessageDialog dialog= new MessageDialog("Configuration File Error!");
                await dialog.ShowAsync();
            }


            if (_networkStatusService.IsOnline())
            {
              
               


                _accessToken = await CurrentEnvironment.Initialize();
                _configuration.AccesToken = _accessToken;

                if (_configuration != null
                   && !_configuration.IsFirstRunSynchronized && DoSync || DoSync) //Zmazat || DoSync ak nechces  sync pri kazdom spusteni, a nie len pri prvom spusteni appky
                    _configuration.IsFirstRunSynchronized = true;

                // Start Sync



                //If Sync Completed

                //Check if the first run was completed



              //  pageTitle.Text = "Welcome to the Windows 8 sample app for Microsoft Dynamics CRM";
                _theMenuItems = new ObservableCollection<MainPageItem>();
                for (int i = 0; i < 7; i++)
                {

                    MainPageItem anItem = new MainPageItem()
                    {
                        Name = _strMenuItems[i]
                    };
                    _theMenuItems.Add(anItem);
                }
                itemsViewSource.Source = _theMenuItems;
            //    progressBar.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Internet Offline");
                await dialog.ShowAsync();
            }
        }
       
        private async void NavigateTo(Type pageType, object parameter)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(pageType, parameter));
        }

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPageItem selItem = (MainPageItem)e.ClickedItem;
            _strItemClicked = selItem.Name;
            if (_strItemClicked.Equals("Tasks"))
            {
                this.NavigateTo(typeof(Tasks), _accessToken);
            }

            else if (_strItemClicked.Equals("Accounts"))
            {
                this.NavigateTo(typeof(Accounts), _configuration);
            }
        }
    }
}
