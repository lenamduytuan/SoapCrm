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
//<snippetModernSoapApp2>
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
namespace ModernSoapApp
{
    public class HttpRequestBuilder
    {
        /// <summary>
        /// Retrieve entity record data from the organization web service. 
        /// </summary>
        /// <param name="accessToken">The web service authentication access token.</param>
        /// <param name="Columns">The entity attributes to retrieve.</param>
        /// <param name="entity">The target entity for which the data should be retreived.</param>
        /// <returns>Response from the web service.</returns>
        /// <remarks>Builds a SOAP HTTP request using passed parameters and sends the request to the server.</remarks>
        public static async Task<string> RetrieveMultipleSOAP(string accessToken, string[] Columns, string entity)
        {
            // Build a list of entity attributes to retrieve as a string.
            string columnsSet = string.Empty;
            foreach (string Column in Columns)
            {
                columnsSet += "<b:string>" + Column + "</b:string>";
            }

            // Default SOAP envelope string. This XML code was obtained using the SOAPLogger tool.
            string xmlSOAP =
             @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
                <s:Body>
                  <RetrieveMultiple xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'>
                    <query i:type='a:QueryExpression' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts'><a:ColumnSet>
                    <a:AllColumns>false</a:AllColumns><a:Columns xmlns:b='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>" + columnsSet +
                   @"</a:Columns></a:ColumnSet><a:Criteria><a:Conditions /><a:FilterOperator>And</a:FilterOperator><a:Filters /></a:Criteria>
                    <a:Distinct>false</a:Distinct><a:EntityName>" + entity + @"</a:EntityName><a:LinkEntities /><a:Orders />
                    <a:PageInfo><a:Count>0</a:Count><a:PageNumber>0</a:PageNumber><a:PagingCookie i:nil='true' />
                    <a:ReturnTotalRecordCount>false</a:ReturnTotalRecordCount>
                    </a:PageInfo><a:NoLock>false</a:NoLock></query>
                  </RetrieveMultiple>
                </s:Body>
              </s:Envelope>";

            // The URL for the SOAP endpoint of the organization web service.
            string url = CurrentEnvironment.CrmServiceUrl + "/XRMServices/2011/Organization.svc/web";

            // Use the RetrieveMultiple CRM message as the SOAP action.
            string SOAPAction = "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/RetrieveMultiple";

            // Create a new HTTP request.
            HttpClient httpClient = new HttpClient();

            // Set the HTTP authorization header using the access token.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Finish setting up the HTTP request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Add("SOAPAction", SOAPAction);
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(xmlSOAP);
            req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

            // Send the request asychronously and wait for the response.
            HttpResponseMessage response;
            response = await httpClient.SendAsync(req);
            var responseBodyAsText = await response.Content.ReadAsStringAsync();

            return responseBodyAsText;
        }
        public static async Task<string> RetrieveMultiple(string accessToken, string[] Columns, string entity, DateTime lastSync)
        {
            // Build a list of entity attributes to retrieve as a string.
            string columnsSet = string.Empty;
            foreach (string Column in Columns)
            {
                columnsSet += "<b:string>" + Column + "</b:string>";
            }

            var avc = lastSync.ToUniversalTime().ToString("s");


            // Default SOAP envelope string. This XML code was obtained using the SOAPLogger tool.
            string xmlSOAP =
             @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
                <s:Body>
                  <RetrieveMultiple xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'>
                    <query i:type='a:QueryExpression' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts'><a:ColumnSet>
                    <a:AllColumns>false</a:AllColumns><a:Columns xmlns:b='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>" + columnsSet +
                   @"</a:Columns></a:ColumnSet><a:Criteria><a:Conditions><a:ConditionExpression><a:AttributeName>createdon</a:AttributeName><a:Operator>GreaterEqual</a:Operator>
              <a:Values xmlns:b='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>
                <b:anyType i:type='c:dateTime' xmlns:c='http://www.w3.org/2001/XMLSchema'>" + lastSync.ToUniversalTime().ToString("s")+ @"</b:anyType>
              </a:Values>
              <a:EntityName i:nil='true' />
            </a:ConditionExpression>
            <a:ConditionExpression>
              <a:AttributeName>modifiedon</a:AttributeName>
              <a:Operator>GreaterEqual</a:Operator>
              <a:Values xmlns:b='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>
                <b:anyType i:type='c:dateTime' xmlns:c='http://www.w3.org/2001/XMLSchema'>" + lastSync.ToUniversalTime().ToString("s") + @"</b:anyType>
              </a:Values>
              <a:EntityName i:nil='true' />
            </a:ConditionExpression>
          </a:Conditions>
          <a:FilterOperator>Or</a:FilterOperator>
          <a:Filters />
        </a:Criteria>
                    <a:Distinct>false</a:Distinct><a:EntityName>" + entity + @"</a:EntityName><a:LinkEntities /><a:Orders />
                    <a:PageInfo><a:Count>0</a:Count><a:PageNumber>0</a:PageNumber><a:PagingCookie i:nil='true' />
                    <a:ReturnTotalRecordCount>false</a:ReturnTotalRecordCount>
                    </a:PageInfo><a:NoLock>false</a:NoLock></query>
                  </RetrieveMultiple>
                </s:Body>
              </s:Envelope>";

            // The URL for the SOAP endpoint of the organization web service.
            string url = CurrentEnvironment.CrmServiceUrl + "/XRMServices/2011/Organization.svc/web";

            // Use the RetrieveMultiple CRM message as the SOAP action.
            string SOAPAction = "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/RetrieveMultiple";

            // Create a new HTTP request.
            HttpClient httpClient = new HttpClient();

            // Set the HTTP authorization header using the access token.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Finish setting up the HTTP request.
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Add("SOAPAction", SOAPAction);
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(xmlSOAP);
            req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml; charset=utf-8");

            // Send the request asychronously and wait for the response.
            HttpResponseMessage response;
            response = await httpClient.SendAsync(req);
            var responseBodyAsText = await response.Content.ReadAsStringAsync();

            return responseBodyAsText;
        }
        #region Public Methods

        /// <summary>
        /// Whoes the am i.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<List<string>> WhoAmI(string accessToken)
        {
            List<string> Settings = new List<string>();
            Settings.Add("");
            Settings.Add("");
            Settings[0] = "";
            Settings[1] = "";
            var soapString = ConstructWhoAmIRequest();
            var soapResponse = string.Empty;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(soapString, Encoding.UTF8, "text/xml");

                using (var response = await client.PostAsync(GetBaseSoapUrl(), content))
                {
                    soapResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new Exception(string.Format("Unable to retrieve response from SOAP Endpoint {0}", response.StatusCode));
                    //TODO ERROR HANDLING
                }
            }

            if (string.IsNullOrWhiteSpace(soapResponse) == false)
            {
                var cleanSoap = RemoveAllNamespaces(soapResponse);
                var results = XDocument.Parse(cleanSoap).Descendants("Results");
                var kvp = results.Descendants().Where(d => d.Name.LocalName.StartsWith("KeyValue"));
                foreach (var pair in kvp)
                {
                    var key = pair.Element("key").Value;
                    var value = pair.Element("value").Value;

                    if (key.Equals("userid", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Settings[0] = Guid.Parse(value).ToString();
                    }

                    //retrieve BU id
                    if (key.Equals("businessunitid", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Settings[1] = Guid.Parse(value).ToString();
                    }

                }
            }
            return Settings;
        }

        /// <summary>
        /// Sets the state request.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="StateCode">The state code.</param>
        /// <param name="StatusCode">The status code.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<string> SetStateRequest(string accessToken, string entity, Guid id, int StateCode, int StatusCode)
        {
            var soapString = ConstructSetStateSoapRequest(id, entity, StateCode.ToString(), StatusCode.ToString());
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(soapString, Encoding.UTF8, "text/xml");
                using (var response = await client.PostAsync(GetBaseSoapUrl(), content))
                {
                    var soapResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new Exception(string.Format("Unable to retrieve response from SOAP Endpoint {0}", response.StatusCode));
                    //TODO ERROR HANDLING
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Fetchs the XML request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessToken">The access token.</param>
        /// <param name="query">The query.</param>
        /// <param name="instanceCreator">The instance creator.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FetchXmlRequest<T>(string accessToken, string xmlEncodedFetchQuery, Func<IEnumerable<XElement>, IEnumerable<T>> entitiesConverter)
        {
            var fetchedRecords = new List<T>();
            var soapResponse = string.Empty;

            //var soapRequest = "";//"<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            //soapRequest += "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            //soapRequest += "<s:Body>";
            //soapRequest += "<RetrieveMultiple xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";
            //soapRequest += "<query i:type=\"a:FetchExpression\" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\">";
            //soapRequest += "<a:Query>";
            //soapRequest += xmlEncodedFetchQuery;
            //soapRequest += "</a:Query>";
            //soapRequest += "</query>";
            //soapRequest += "</RetrieveMultiple>";
            //soapRequest += "</s:Body>";
            //soapRequest += "</s:Envelope>";
            var soapRequest = "<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body>";
            soapRequest += "<Execute xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services' xmlns:i='http://www.w3.org/2001/XMLSchema-instance' >";
            soapRequest += "<request i:type='a:RetrieveMultipleRequest' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts'>";
            soapRequest += "<a:Parameters xmlns:b='http://schemas.datacontract.org/2004/07/System.Collections.Generic'>";
            soapRequest += "<a:KeyValuePairOfstringanyType>";
            soapRequest += "<b:key>Query</b:key>";
            soapRequest += "<b:value i:type='a:FetchExpression'>";
            soapRequest += "<a:Query>";
            soapRequest += xmlEncodedFetchQuery;
            soapRequest += "</a:Query>";
            soapRequest += "</b:value>";
            soapRequest += "</a:KeyValuePairOfstringanyType>";
            soapRequest += "</a:Parameters>";
            soapRequest += "<a:RequestId i:nil='true'/>";
            soapRequest += "<a:RequestName>RetrieveMultiple</a:RequestName>";
            soapRequest += "</request>";
            soapRequest += "</Execute>";
            soapRequest += "</s:Body></s:Envelope>";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                ///validate URL
                using (var response = await client.PostAsync(GetBaseSoapUrl(), content))
                {
                    soapResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new Exception(string.Format("Unable to retrieve response from SOAP Endpoint {0}", response.StatusCode));
                    //TODO ERROR HANDLING
                }

                if (string.IsNullOrEmpty(soapResponse) == false)
                {
                    var cleanSoap = RemoveAllNamespaces(soapResponse);
                    var entities = XDocument.Parse(cleanSoap)
                        .Descendants("Results")
                        .Descendants("value")
                        .Descendants("Entities");

                    return entitiesConverter(entities);
                }

                return fetchedRecords;
            }

        }

        /// <summary>
        /// Encodes the XML string.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static string EncodeXmlString(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return string.Empty;

            return System.Net.WebUtility.HtmlEncode(xml);
        }
        #endregion

        #region Private Methods

        private string ConstructSetStateSoapRequest(Guid Id, string EntityLogicalName, string state, string status)
        {
            string body = "";
            body += "<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>";
            body += "<s:Body>";
            body += "<Execute xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'>";
            body += "<request i:type='b:SetStateRequest' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts' xmlns:b='http://schemas.microsoft.com/crm/2011/Contracts'>";
            body += "<a:Parameters xmlns:c='http://schemas.datacontract.org/2004/07/System.Collections.Generic'>";
            body += "<a:KeyValuePairOfstringanyType>";
            body += "<c:key>EntityMoniker</c:key>";
            body += "<c:value i:type='a:EntityReference'>";
            body += "<a:Id>" + Id.ToString() + "</a:Id>";
            body += "<a:LogicalName>" + EntityLogicalName + "</a:LogicalName>";
            body += "<a:Name i:nil='true' />";
            body += "</c:value>";
            body += "</a:KeyValuePairOfstringanyType>";
            body += "<a:KeyValuePairOfstringanyType>";
            body += "<c:key>State</c:key>";
            body += "<c:value i:type='a:OptionSetValue'>";
            body += "<a:Value>" + state + "</a:Value>";
            body += "</c:value>";
            body += "</a:KeyValuePairOfstringanyType>";
            body += "<a:KeyValuePairOfstringanyType>";
            body += "<c:key>Status</c:key>";
            body += "<c:value i:type='a:OptionSetValue'>";
            body += "<a:Value>" + status + "</a:Value>";
            body += "</c:value>";
            body += "</a:KeyValuePairOfstringanyType>";
            body += "</a:Parameters>";
            body += "<a:RequestId i:nil='true' />";
            body += "<a:RequestName>SetState</a:RequestName>";
            body += "</request>";
            body += "</Execute>";
            body += "</s:Body>";
            body += "</s:Envelope>";
            return body;
        }

        private string ConstructWhoAmIRequest()
        {
            string request = @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>" +
                               "<s:Body>" +
                                   "<Execute xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'> " +
                                       "<request i:type='b:WhoAmIRequest' xmlns:a='http://schemas.microsoft.com/xrm/2011/Contracts' xmlns:b='http://schemas.microsoft.com/crm/2011/Contracts'> " +
                                           "<a:Parameters xmlns:c='http://schemas.datacontract.org/2004/07/System.Collections.Generic' /> " +
                                           "<a:RequestId i:nil='true' /> " +
                                           "<a:RequestName>WhoAmI</a:RequestName> " +
                                       "</request>" +
                                   "</Execute>" +
                               "</s:Body>" +
                            "</s:Envelope>";
            return request;
        }

        private Uri GetBaseSoapUrl()
        {

            return new Uri(CurrentEnvironment.CrmServiceUrl + "/XRMServices/2011/Organization.svc/web");
        }

        private string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));
            return xmlDocumentWithoutNs.ToString();
        }

        private XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;
                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        #endregion

    }
}
//</snippetModernSoapApp2>
