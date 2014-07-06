// =====================================================================
//
//  This file is part of the Microsoft Dynamics CRM SDK Code Samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or online documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//
// =====================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.UI.Popups;
using ModernSoapApp.Common;
using ModernSoapApp.Helper.Entities;
using ModernSoapApp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using Microsoft.Preview.WindowsAzure.ActiveDirectory.Authentication;
using ModernSoapApp.ViewModels;
using Windows.UI.Core;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ModernSoapApp.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Accounts : ModernSoapApp.Common.LayoutAwarePage
    {
        AccountsViewModel accountsVM;

        public Accounts()
        {
            this.InitializeComponent();
            accountsVM = new AccountsViewModel();
        }
        private string _accessToken;
        private Configuration _configuration;
       protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _configuration = (Configuration)e.Parameter;
            if (_configuration == null)
            {
                MessageDialog dialog = new MessageDialog("Error on Sending parameters no data will be displayed!");
                return;
            }
            _accessToken = _configuration.AccesToken;
            GetAccountsData();
        }

        public void NavigateToMainScreen()
        {
            
            this.NavigateTo(typeof(MainPage), _configuration);

        }
        private async void NavigateTo(Type pageType, object parameter)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(pageType, parameter));
        }

        /// <summary>
        /// Method to bind Accounts Data to Accounts ListView
        /// </summary>
        public async void GetAccountsData()
        {
          var accs=  await accountsVM.AccountsRetrieveCRM(_accessToken,DateTime.Now.AddMonths(-1));
            this.listBoxAccounts.ItemsSource = accountsVM.Accounts;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
