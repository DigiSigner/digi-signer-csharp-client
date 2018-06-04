using DigiSigner.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DigiSigner.Client.Tests
{
    public class SignatureRequestForTemplateFullTests : DigiSignerClientTestsHelper
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
         * Test sending signature request for template.
         * Curl example:
         * {"documents" : [
         * {"document_id": "6586b79c-60a9-4986-a96d-2b8841cfb567",
         * "title": "Sample title", "subject": "Sample subject", "message": "Sample message",
         * "signers": [{"email": "signer_1@example.com"},{"email": "signer_2@example.com"}]}]}
         */
        [Fact]
        public void SendSignatureRequestForTemplateTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document();
            document.ID = idOfTheTemplate;
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
         *   "title": "Sample title", 
         *   "subject": "Sample subject", 
         *   "message": "Sample message",
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
         *      ]},
         *      {
         *        "email": "signer_2@example.com", 
         *        "role": "Manager", 
         *        "existing_fields": [
         *          {
         *            "api_id": "b96211e4-08bc-4d6d-8498-30a991ff39f3", 
         *            "content": "Mary Brown",
         *            "label": "Please sign", 
         *            "required": true, 
         *            "read_only": false
         *          },
         *          {
         *            "api_id": "5ac9c8c5-4f4d-4a1b-b2e1-4eb07f9f579f", 
         *            "content": "Mary Brown", 
         *            "label": "Your name",
         *            "required": false, 
         *            "read_only": false
         *          }]}]}]}
         */
        [Fact]
        public void SendSignatureRequestWithExistingFieldsForTemplateTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document(relativePathToFileOfTheDocument);
            document.ID = idOfTheDocumentWithFileds;
            document.Title = documentValues["title"];
            document.Subject = documentValues["subject"];
            document.Message = documentValues["message"];

            // add first signer
            Signer signer1 = new Signer(signers["signer1"]["email"]);
            signer1.Role = "Employee";
            signer1.Order = 1;

            // add field for first signer
            ExistingField field1 = new ExistingField((string)existingField1["fieldId"]);
            field1.Content = (string)existingField1["content"];
            field1.Label = (string)existingField1["label"];
            field1.Required = (bool)existingField1["required"];
            field1.ReadOnly = (bool)existingField1["readonly"];
            signer1.ExistingFields.Add(field1);

            // add second field to first signer
            ExistingField field2 = new ExistingField((string)existingField2["fieldId"]);
            field2.Content = (string)existingField2["content"];
            field2.Label = (string)existingField2["label"];
            field2.Required = (bool)existingField2["required"];
            field2.ReadOnly = (bool)existingField2["readonly"];
            signer1.ExistingFields.Add(field2);

            // add second signer
            Signer signer2 = new Signer(signers["signer2"]["email"]);
            signer2.Role = "Manager";
            signer2.Order = 2;

            // add field for second signer
            ExistingField field3 = new ExistingField((string)existingField3["fieldId"]);
            field3.Content = (string)existingField3["content"];
            field3.Label = (string)existingField3["label"];
            field3.Required = (bool)existingField3["required"];
            field3.ReadOnly = (bool)existingField3["readonly"];
            signer2.ExistingFields.Add(field3);

            // add second field to second signer
            ExistingField field4 = new ExistingField((string)existingField4["fieldId"]);
            field4.Content = (string)existingField4["content"];
            field4.Label = (string)existingField4["label"];
            field4.Required = (bool)existingField4["required"];
            field4.ReadOnly = (bool)existingField4["readonly"];
            signer2.ExistingFields.Add(field4);

            document.Signers.Add(signer1);
            document.Signers.Add(signer2);
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
