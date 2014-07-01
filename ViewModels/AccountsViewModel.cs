// =====================================================================
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
// =====================================================================

using System.IO;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ModernSoapApp;
using ModernSoapApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLite;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;



namespace ModernSoapApp.ViewModels
{
    public class AccountsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<AccountsModel> accounts;
        public ObservableCollection<AccountsModel> Accounts
        {
            get { return accounts; }
            set
            {
                if (value != accounts)
                {
                    accounts = value;
                    NotifyPropertyChanged("Accounts");
                }
            }
        }

        /// <summary>
        /// Fetch Accounts details.
        /// Extracts Accounts details from XML response and binds data to Observable Collection.
        /// </summary>    
        public async Task<ObservableCollection<AccountsModel>> AccountsRetrieveCRM(string AccessToken,DateTime lastSync)
        {
        
            var AccountsResponseBody = await HttpRequestBuilder.RetrieveMultipleSOAP(AccessToken, new string[] { "name", "emailaddress1", "telephone1" }, "account");

            ObservableCollection<AccountsModel> Accounts = new ObservableCollection<AccountsModel>();

            // Converting response string to xDocument.
            XDocument xdoc = XDocument.Parse(AccountsResponseBody.ToString(), LoadOptions.None);
            XNamespace s = "http://schemas.xmlsoap.org/soap/envelope/";//Envelop namespace s
            XNamespace a = "http://schemas.microsoft.com/xrm/2011/Contracts";//a namespace
            XNamespace b = "http://schemas.datacontract.org/2004/07/System.Collections.Generic";//b namespace

            foreach (var entity in xdoc.Descendants(s + "Body").Descendants(a + "Entities").Descendants(a + "Entity"))
            {
                AccountsModel account = new AccountsModel();
       
                foreach (var KeyvaluePair in entity.Descendants(a + "KeyValuePairOfstringanyType"))
                {
                    if (KeyvaluePair.Element(b + "key").Value == "name")
                    {
                        account.Name = KeyvaluePair.Element(b + "value").Value;
                    }
                    else if (KeyvaluePair.Element(b + "key").Value == "emailaddress1")
                    {
                        account.Email = KeyvaluePair.Element(b + "value").Value;
                    }
                    else if (KeyvaluePair.Element(b + "key").Value == "telephone1")
                    {
                        account.Phone = KeyvaluePair.Element(b + "value").Value;
                    }
                    else if (KeyvaluePair.Element(b + "key").Value == "accountid")
                    {
                        account.Accountid = new Guid(KeyvaluePair.Element(b + "value").Value);
                    }
                }
                Accounts.Add(account);
            
            }
            this.Accounts = Accounts;
           await createAccountTable();
            Accounts.Clear();
            var av = GetAllAccountsDB().Result;
            this.Accounts = await GetAllAccountsDB();
            Accounts = await GetAllAccountsDB();
           
            return Accounts; // Accounts;
        }
        public async Task<ObservableCollection<AccountsModel>> GetAllAccountsDB()
        {
            ObservableCollection<AccountsModel> _accounts_DB = new ObservableCollection<AccountsModel>();
            try
            {
                var dbpath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
                using (var db = new SQLite.SQLiteConnection(dbpath))
                {
                 
                    var AccountsDB = db.Table<AccountsModel>().Where(a => a.Name.Contains("a"));

                    foreach (AccountsModel accountModel in AccountsDB)
                    {
                        _accounts_DB.Add(accountModel);
                    }
                 
                    db.Commit();
                    db.Dispose();
                    db.Close();
                    //var line = new MessageDialog("Records Inserted");
                    //await line.ShowAsync();
                }


            }
            catch (SQLiteException)
            {

            }
            return _accounts_DB;
        }

        private async Task AccountStore(ObservableCollection<AccountsModel> account)
        {
            try
            {
                var dbpath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
                using (var db = new SQLite.SQLiteConnection(dbpath))
                {
                    foreach (AccountsModel accountM in Accounts)
                    {
                        var accountInDB = db.Find<AccountsModel>(accountM.Accountid);
                        if (accountInDB != null)
                        {
                            db.Delete(accountM);
                        }
                        db.Insert(accountM);
                    }
                    // Create the tables if they don't exist
                    db.Commit();
                    db.Dispose();
                    db.Close();
                    //var line = new MessageDialog("Records Inserted");
                    //await line.ShowAsync();
                }


            }
            catch (SQLiteException)
            {

            }


        }

        /// <summary>
        /// Create Table for accout method
        /// </summary>
        /// 
        private async Task<bool> createAccountTable()
        {
            try
            {
                var dbpath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
                using (var db = new SQLite.SQLiteConnection(dbpath))
                {

                    // Create the tables if they don't exist
                    db.CreateTable<AccountsModel>();
                    db.Commit();

                    db.Dispose();
                    db.Close();
                }
                //var line = new MessageDialog("Table Created");
                //await line.ShowAsync();
            }

            catch (SQLiteException exLite)
            {
                throw new Exception(exLite.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            try
            {


                var dbpath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "data.db3");
                using (var db = new SQLite.SQLiteConnection(dbpath))
                {
                    AccountStore(Accounts);
                    // Create the tables if they don't exist
                    db.Commit();
                    db.Dispose();
                    db.Close();
                    //var line = new MessageDialog("Records Inserted");
                    //await line.ShowAsync();
                }


            }
            catch (SQLiteException)
            {

            }
            return true;

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
    public static class PropertyHelper<T>
    {
        public static PropertyInfo GetProperty<TValue>(
            Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

