using DigiSigner.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DigiSigner.Client.Tests
{
    public class SignatureRequestFullTests : DigiSignerClientTestsHelper
    {
        protected Dictionary<string, object> existingField1 = new Dictionary<string, object>
        {
            {"fieldId",  "b7f9bf0d-c616-4d9c-897f-3682b62e8f7d"},
            {"content",  "Sample content 1"},
            {"required", true }
        };

        protected Dictionary<string, object> existingField2 = new Dictionary<string, object>
        {
            {"fieldId",  "cc6a2a9c-54b4-43ac-9778-93192b2ab158"},
            {"content",  "Sample content 2"},
            {"required", true }
        };


        /*
         * Tests send signature request.
         * Curl example:
         * {"documents" : [
         * {"document_id": "06c4d320-d6c5-492b-b343-8482338ef9d0",
         * "title": "Sample title",
         * "subject": "Sample subject",
         * "message": "Sample message",
         * "signers": [*{"email": "signer_1@example.com"},{"email": "signer_2@example.com"}]}]}
         */
        [Fact]
        public void SendSignatureRequestTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            // add document with possible attributes
            Document document = new Document(relativePathToFileOfTheDocument);
            document.Title = documentValues["title"];
            document.Subject = documentValues["subject"];
            document.Message = documentValues["message"];

            Signer signer1 = new Signer(signers["signer1"]["email"]);
            signer1.Order = 1;

            Signer signer2 = new Signer(signers["signer2"]["email"]);
            signer2.Order = 2;

            document.Signers.Add(signer1);
            document.Signers.Add(signer2);

            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, true);

            // get and validate signature request from database
            String signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(signatureRequestId);

            Assert.Equal(2, createdSignatureRequest.Documents[0].Signers.Count);
            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, true);
        }


        /*
         * Tests send signature request as bundle.
         * Curl example:
         * {
         * {"send_documents_as_bundle": true,
         * "bundle_title": "Bundle title",
         * "bundle_subject": "My subject",
         * "bundle_message": "My message",
         *
         * "documents" : [
         * {"document_id": "06c4d320-d6c5-492b-b343-8482338ef9d0",
         * "signers": [*{"email": "signer_1@example.com"},{"email": "signer_2@example.com"}]}]}
         */
        [Fact]
        public void SendSignatureRequestTestAsBundle()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;
            signatureRequest.SendDocumentsAsBundle = true;
            signatureRequest.BundleTitle = "Bundle title";
            signatureRequest.BundleSubject = "My subject";
            signatureRequest.BundleMessage = "My message";

            // add document with possible attributes
            Document document = new Document(relativePathToFileOfTheDocument);

            Signer signer1 = new Signer(signers["signer1"]["email"]);
            signer1.Order = 1;

            Signer signer2 = new Signer(signers["signer2"]["email"]);
            signer2.Order = 2;

            document.Signers.Add(signer1);
            document.Signers.Add(signer2);

            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, false);

            // get and validate signature request from database
            String signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(signatureRequestId);

            Assert.Equal(2, createdSignatureRequest.Documents[0].Signers.Count);
            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, false);
        }

        /*
         * Test signature request for template with fields.
         * Curl example:
         * {"documents" : [{"document_id": "6586b79c-60a9-4986-a96d-2b8841cfb567",
         * "signers": [{"email": "signer_1@example.com", "role": "Signer 1",
         * "existing_fields": [
         * {"api_id": "d9fbf81b-cfa1-47cd-bc3e-3980131af733", "content": "Sample content 1"},
         * {"api_id": "00b25bcc-7909-4174-b18c-04ae2fb01775", "content": "Sample content 2"}
         * ]}]}]}
         */

        [Fact]
        public void SendSignatureRequestWithExistingFieldsTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document(relativePathToFileOfTheDocument);
            document.ID = idOfTheDocumentWithExisitingFileds;
            Signer signer = new Signer(signers["signer1"]["email"]);
            signer.Role = "Signer 1";
            // add fields
            ExistingField field1 = new ExistingField((string)existingField1["fieldId"]);
            field1.Content = (string)existingField1["content"];
            field1.Required = (bool)existingField1["required"];
            signer.ExistingFields.Add(field1);

            // add second field to signer
            ExistingField field2 = new ExistingField((string)existingField2["fieldId"]);
            field2.Content = (string)existingField2["content"];
            field2.Required = (bool)existingField2["required"];
            signer.ExistingFields.Add(field2);

            document.Signers.Add(signer);
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, false);

            // get and validate signature request from database
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(
                    signatureRequestResponse.SignatureRequestId);
            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, false);

            // get and validate fields from database
            Document expectedDocument = signatureRequest.Documents[0];
            DocumentFields documentFields = client.GetDocumentFields(createdSignatureRequest.Documents[0].ID);
            ValidateDocumentFields(expectedDocument, documentFields);
        }
    }
}
