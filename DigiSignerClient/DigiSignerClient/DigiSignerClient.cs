﻿using System;
using System.Collections.Generic;
using System.IO;
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
            using (WebClient webClient = new WebClient())
            {
                addAuthInfo(webClient.Headers);

                byte[] result = webClient.UploadFile(Config.getDocumentUrl(serverUrl), filename);

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    Encoding.ASCII.GetString(result)
                )[Config.PARAM_DOC_ID];
            }
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

        /// <summary>
        /// Download the document by ID.
        /// </summary>
        /// <param name="documentId">ID of document.</param>
        /// <param name="filename">the name of the document file to be saved.</param>
        public void getDocumentById(string documentId, string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                addAuthInfo(webClient.Headers);

                webClient.DownloadFile(Config.getDocumentUrl(serverUrl) + "/" + documentId, filename);
            }
        }

        /// <summary>
        /// Adds content to the document after given document ID.
        /// </summary>
        /// <param name="documentId">documentId to insert content.</param>
        /// <param name="signatures">signatures will be rendered on the document.</param>
        public void addContentToDocument(string documentId, List<Signature> signatures)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getContentUrl(serverUrl, documentId));

            addAuthInfo(webRequest.Headers);

            webRequest.Method = "Post";
           
            wrireBodyRequest(
                webRequest,
                JsonConvert.SerializeObject(new DocumentContent(signatures), Formatting.Indented)
            );

            WebResponse response = webRequest.GetResponse();
        }

        /// <summary>
        /// Returns document fields for a document.
        /// </summary>
        /// <param name="documentId">Id of the document.</param>
        /// <returns>Document's fields</returns>
        public DocumentFields getDocumentFields(string documentId)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getFieldsUrl(serverUrl, documentId));

            addAuthInfo(webRequest.Headers);

            webRequest.Method = "Get";

            return readFieldsFromBody<DocumentFields>(
                webRequest.GetResponse()
            );
        }

        /// <summary>
        /// The get signature request to check information about signature such as signature is completed
        /// and IDs of signature request and documents.
        /// </summary>
        /// <param name="signatureRequestId">ID of the signature request.</param>
        /// <returns>SignatureRequest with filled IDs and signature request data.</returns>
        public SignatureRequest getSignatureRequest(string signatureRequestId)
        {
            String url = Config.getSignatureRequestsUrl(serverUrl) + "/" + signatureRequestId;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            addAuthInfo(webRequest.Headers);

            webRequest.Method = "Get";

            return readFieldsFromBody<SignatureRequest>(
                webRequest.GetResponse()
            );
        }

        /// <summary>
        /// Sends the signature request to the server.
        /// </summary>
        /// <param name="signatureRequest">signatureRequest filled signature request with required data.</param>
        /// <returns>result with sent signature request ID.</returns>
        public SignatureRequest sendSignatureRequest(SignatureRequest signatureRequest)
        {
            foreach (Document document in signatureRequest.Documents)
            {
                if (document.ID == null)
                {
                    document.ID = uploadDocument(document.FileName);
                }
            }

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getSignatureRequestsUrl(serverUrl));

            addAuthInfo(webRequest.Headers);

            webRequest.Method = "Post";

            wrireBodyRequest(
                webRequest,
                JsonConvert.SerializeObject(signatureRequest, Formatting.Indented)
            );

            return readFieldsFromBody<SignatureRequest>(
                webRequest.GetResponse()
            );
        }

        private void wrireBodyRequest(HttpWebRequest request, string json)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(json);

            request.ContentLength = buffer.Length;
            request.ContentType = "application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(buffer, 0, buffer.Length);
                dataStream.Flush();

                dataStream.Close();
            }
        }

        private T readFieldsFromBody<T>(WebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                byte[] buffer = new byte[response.ContentLength];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                string json = Encoding.ASCII.GetString(buffer);

                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
