using DigiSigner.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DigiSigner.Client.Tests
{
    public class SignatureRequestForTemplateSimpleTests : DigiSignerClientTestsHelper
    {
        private Dictionary<string, object> existingField1 = new Dictionary<string, object>
        {
            {"fieldId",  "f2eb6940-1797-4fef-ae7d-39bb76cbb2a7"},
            {"content",  "Sample content 1"},
            {"label", "Please sign"},
            {"required", true },
            {"readonly", false }
        };

        private Dictionary<string, object> existingField2 = new Dictionary<string, object>
        {
            {"fieldId",  "8e76a737-84ed-4cfa-959f-3500c7490de5"},
            {"content",  "James Williams"},
            {"label", "Your name"},
            {"required", true },
            {"readonly", false }
        };

        private Dictionary<string, object> existingField3 = new Dictionary<string, object>
        {
            {"fieldId",  "585a5230-eb43-4574-b8cd-0300249041de"},
            {"content",  "Mary Brown"},
            {"label", "Please sign"},
            {"required", true },
            {"readonly", false }
        };

        private Dictionary<string, object> existingField4 = new Dictionary<string, object>
        {
            {"fieldId",  "5fa4b24b-2775-4276-a1c5-8a6b0af58931"},
            {"content",  "Mary Brown"},
            {"label", "Your name"},
            {"required", false },
            {"readonly", false }
        };

        /*
         * Test sending signature simple request for template.
         * Curl example:
         * {"documents" : [
         * {"document_id": "6586b79c-60a9-4986-a96d-2b8841cfb567",
         * "signers": [{"email": "signer_1@example.com"}]}]}
         */
        [Fact]
        public void SendSignatureRequestForTemplateTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document();
            document.ID = idOfTheTemplate;

            Signer signer = new Signer(signers["signer1"]["email"]);

            document.Signers.Add(signer);
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, false);

            // get and validate signature request from database
            String signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(signatureRequestId);

            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, false);
        }

        /*
         * Test sending signature request with fields for template.
         * Curl example:
         * {"documents" : [
         * {
         *   "document_id": "79fbdbc7-dbac-424d-8e2e-507ea4ebb53f",
         *   "signers": [
         *     {
         *       "email": "signer_1@example.com",
         *       "role": "Employee",
         *       "existing_fields": [
         *          {
         *            "api_id": "d9fbf81b-cfa1-47cd-bc3e-3980131af733", 
         *            "content": "Sample content 1", 
         *            "label": "Please sign",
         *            "required": true, 
         *            "read_only": false },
         *          {
         *            "api_id": "00b25bcc-7909-4174-b18c-04ae2fb01775", 
         *            "content": "James Williams", 
         *            "label": "Your name",
         *            "required": true, 
         *            "read_only": false 
         *          }
         *      ]}]}]}
         */
        [Fact]
        public void SendSignatureRequestWithExistingFieldsForTemplateTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document(relativePathToFileOfTheDocument);
            document.ID = idOfTheDocumentWithFileds;
            
            // add first signer
            Signer signer = new Signer(signers["signer1"]["email"]);
            signer.Role = "Employee";
            signer.Order = 1;

            // add field for first signer
            ExistingField field1 = new ExistingField((string)existingField1["fieldId"]);
            field1.Content = (string)existingField1["content"];
            field1.Label = (string)existingField1["label"];
            field1.Required = (bool)existingField1["required"];
            field1.ReadOnly = (bool)existingField1["readonly"];
            signer.ExistingFields.Add(field1);

            // add second field to first signer
            ExistingField field2 = new ExistingField((string)existingField2["fieldId"]);
            field2.Content = (string)existingField2["content"];
            field2.Label = (string)existingField2["label"];
            field2.Required = (bool)existingField2["required"];
            field2.ReadOnly = (bool)existingField2["readonly"];
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
