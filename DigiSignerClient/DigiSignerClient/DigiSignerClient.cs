using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace DigiSigner.Client
{
    public class DigiSignerClient
    {
        private readonly string apiKey;
        private readonly string serverUrl;

        public DigiSignerClient(string apiKey)
        {
            this.apiKey = apiKey;
            serverUrl = Config.DEFAULT_SERVER_URL;
        }

        private void addAuthInfo(WebHeaderCollection headers)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey + ":"));
            headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
        }

        /// <summary>
        /// Upload document and returns ID of document.
        /// </summary>
        /// <param name="document">document to upload.</param>
        /// <returns>ID of uploaded document.</returns>
        public string uploadDocument(string filename)
        {
            WebClient webClient = new WebClient();

            addAuthInfo(webClient.Headers);

            byte[] result = webClient.UploadFile(Config.getDocumentUrl(serverUrl), filename);

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(
                Encoding.ASCII.GetString(result)
            )[Config.PARAM_DOC_ID];           
        }

        /// <summary>
        /// Deletes document by document ID.
        /// </summary>
        /// <param name="documentId">ID of deleteded document.</param>
        public void deleteDocument(string documentId)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getDeleteDocumentUrl(serverUrl, documentId));

            addAuthInfo(webRequest.Headers);
            
            webRequest.Method = "Delete";
            webRequest.GetResponse();
        }

        public void getDocumentById(string documentId, string filename)
        {
            WebClient webClient = new WebClient();

            addAuthInfo(webClient.Headers);

            webClient.DownloadFile(Config.getDocumentUrl(serverUrl)+"/"+documentId, filename);
        }
    }
}
