using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

using DigiSigner.Client;

namespace XUnitTestDigiSignerClient
{
    public class DigiSignerClientTests
    {
        [Fact]
        public void UploadDocument()
        {
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            string res = client.uploadDocument("../../../1.pdf");

            client.getDocumentById(res, "../../../2.pdf");

            client.deleteDocument(res);
        }

        [Fact]
        public void AddContentToDocument()
        {
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            string res = client.uploadDocument("../../../document.pdf");

            Signature signature1 = new Signature
            {
                Page = 1,
                Rectangle = new int[] { 0, 0, 200, 100 },
                Image = Encoding.ASCII.GetBytes("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"),
                DrawCoordinates = "[{\"lx\":202,\"ly\":76,\"mx\":202,\"my\":75},{\"lx\":202,\"ly\":79,\"mx\":202,\"my\":76},{\"lx\":202,\"ly\":82,\"mx\":202,\"my\":79},{\"lx\":201,\"ly\":84,\"mx\":202,\"my\":82},{\"lx\":200,\"ly\":87,\"mx\":201,\"my\":84},{\"lx\":199,\"ly\":90,\"mx\":200,\"my\":87},{\"lx\":198,\"ly\":93,\"mx\":199,\"my\":90},{\"lx\":198,\"ly\":97,\"mx\":198,\"my\":93},{\"lx\":197,\"ly\":101,\"mx\":198,\"my\":97},{\"lx\":195,\"ly\":107,\"mx\":197,\"my\":101},{\"lx\":193,\"ly\":112,\"mx\":195,\"my\":107},{\"lx\":191,\"ly\":118,\"mx\":193,\"my\":112},{\"lx\":188,\"ly\":122,\"mx\":191,\"my\":118},{\"lx\":186,\"ly\":127,\"mx\":188,\"my\":122},{\"lx\":184,\"ly\":132,\"mx\":186,\"my\":127},{\"lx\":183,\"ly\":136,\"mx\":184,\"my\":132},{\"lx\":180,\"ly\":141,\"mx\":183,\"my\":136},{\"lx\":178,\"ly\":144,\"mx\":180,\"my\":141},{\"lx\":174,\"ly\":149,\"mx\":178,\"my\":144},{\"lx\":171,\"ly\":154,\"mx\":174,\"my\":149},{\"lx\":169,\"ly\":158,\"mx\":171,\"my\":154},{\"lx\":165,\"ly\":161,\"mx\":169,\"my\":158},{\"lx\":163,\"ly\":164,\"mx\":165,\"my\":161},{\"lx\":161,\"ly\":165,\"mx\":163,\"my\":164},{\"lx\":160,\"ly\":166,\"mx\":161,\"my\":165},{\"lx\":159,\"ly\":166,\"mx\":160,\"my\":166},{\"lx\":156,\"ly\":167,\"mx\":159,\"my\":166},{\"lx\":155,\"ly\":167,\"mx\":156,\"my\":167},{\"lx\":154,\"ly\":168,\"mx\":155,\"my\":167},{\"lx\":152,\"ly\":168,\"mx\":154,\"my\":168},{\"lx\":148,\"ly\":168,\"mx\":152,\"my\":168},{\"lx\":145,\"ly\":168,\"mx\":148,\"my\":168},{\"lx\":144,\"ly\":168,\"mx\":145,\"my\":168},{\"lx\":142,\"ly\":168,\"mx\":144,\"my\":168},{\"lx\":141,\"ly\":167,\"mx\":142,\"my\":168},{\"lx\":139,\"ly\":166,\"mx\":141,\"my\":167},{\"lx\":138,\"ly\":164,\"mx\":139,\"my\":166},{\"lx\":134,\"ly\":159,\"mx\":138,\"my\":164},{\"lx\":131,\"ly\":153,\"mx\":134,\"my\":159},{\"lx\":126,\"ly\":143,\"mx\":131,\"my\":153},{\"lx\":125,\"ly\":138,\"mx\":126,\"my\":143},{\"lx\":125,\"ly\":135,\"mx\":125,\"my\":138},{\"lx\":125,\"ly\":131,\"mx\":125,\"my\":135},{\"lx\":126,\"ly\":129,\"mx\":125,\"my\":131},{\"lx\":127,\"ly\":128,\"mx\":126,\"my\":129},{\"lx\":127,\"ly\":127,\"mx\":127,\"my\":128},{\"lx\":128,\"ly\":127,\"mx\":127,\"my\":127},{\"lx\":173,\"ly\":88,\"mx\":173,\"my\":87},{\"lx\":173,\"ly\":90,\"mx\":173,\"my\":88},{\"lx\":173,\"ly\":91,\"mx\":173,\"my\":90},{\"lx\":173,\"ly\":92,\"mx\":173,\"my\":91},{\"lx\":172,\"ly\":93,\"mx\":173,\"my\":92},{\"lx\":171,\"ly\":94,\"mx\":172,\"my\":93},{\"lx\":169,\"ly\":95,\"mx\":171,\"my\":94},{\"lx\":168,\"ly\":96,\"mx\":169,\"my\":95},{\"lx\":166,\"ly\":96,\"mx\":168,\"my\":96},{\"lx\":164,\"ly\":98,\"mx\":166,\"my\":96},{\"lx\":161,\"ly\":99,\"mx\":164,\"my\":98},{\"lx\":158,\"ly\":99,\"mx\":161,\"my\":99},{\"lx\":155,\"ly\":99,\"mx\":158,\"my\":99},{\"lx\":152,\"ly\":99,\"mx\":155,\"my\":99},{\"lx\":149,\"ly\":99,\"mx\":152,\"my\":99},{\"lx\":147,\"ly\":99,\"mx\":149,\"my\":99},{\"lx\":144,\"ly\":99,\"mx\":147,\"my\":99},{\"lx\":142,\"ly\":99,\"mx\":144,\"my\":99},{\"lx\":141,\"ly\":99,\"mx\":142,\"my\":99},{\"lx\":139,\"ly\":99,\"mx\":141,\"my\":99},{\"lx\":138,\"ly\":99,\"mx\":139,\"my\":99},{\"lx\":137,\"ly\":98,\"mx\":138,\"my\":99},{\"lx\":137,\"ly\":97,\"mx\":137,\"my\":98},{\"lx\":135,\"ly\":96,\"mx\":137,\"my\":97},{\"lx\":135,\"ly\":94,\"mx\":135,\"my\":96},{\"lx\":134,\"ly\":93,\"mx\":135,\"my\":94},{\"lx\":133,\"ly\":91,\"mx\":134,\"my\":93},{\"lx\":133,\"ly\":89,\"mx\":133,\"my\":91},{\"lx\":133,\"ly\":87,\"mx\":133,\"my\":89},{\"lx\":133,\"ly\":86,\"mx\":133,\"my\":87},{\"lx\":133,\"ly\":84,\"mx\":133,\"my\":86},{\"lx\":133,\"ly\":83,\"mx\":133,\"my\":84},{\"lx\":134,\"ly\":81,\"mx\":133,\"my\":83},{\"lx\":136,\"ly\":80,\"mx\":134,\"my\":81},{\"lx\":138,\"ly\":78,\"mx\":136,\"my\":80},{\"lx\":142,\"ly\":78,\"mx\":138,\"my\":78},{\"lx\":147,\"ly\":76,\"mx\":142,\"my\":78},{\"lx\":153,\"ly\":76,\"mx\":147,\"my\":76},{\"lx\":161,\"ly\":76,\"mx\":153,\"my\":76},{\"lx\":168,\"ly\":76,\"mx\":161,\"my\":76},{\"lx\":178,\"ly\":76,\"mx\":168,\"my\":76},{\"lx\":185,\"ly\":76,\"mx\":178,\"my\":76},{\"lx\":193,\"ly\":76,\"mx\":185,\"my\":76},{\"lx\":200,\"ly\":76,\"mx\":193,\"my\":76},{\"lx\":207,\"ly\":77,\"mx\":200,\"my\":76},{\"lx\":213,\"ly\":78,\"mx\":207,\"my\":77},{\"lx\":217,\"ly\":79,\"mx\":213,\"my\":78},{\"lx\":222,\"ly\":81,\"mx\":217,\"my\":79},{\"lx\":225,\"ly\":83,\"mx\":222,\"my\":81},{\"lx\":229,\"ly\":85,\"mx\":225,\"my\":83},{\"lx\":232,\"ly\":86,\"mx\":229,\"my\":85},{\"lx\":233,\"ly\":88,\"mx\":232,\"my\":86},{\"lx\":235,\"ly\":89,\"mx\":233,\"my\":88},{\"lx\":236,\"ly\":89,\"mx\":235,\"my\":89},{\"lx\":238,\"ly\":90,\"mx\":236,\"my\":89},{\"lx\":239,\"ly\":90,\"mx\":238,\"my\":90},{\"lx\":240,\"ly\":91,\"mx\":239,\"my\":90},{\"lx\":241,\"ly\":91,\"mx\":240,\"my\":91},{\"lx\":242,\"ly\":91,\"mx\":241,\"my\":91},{\"lx\":243,\"ly\":91,\"mx\":242,\"my\":91},{\"lx\":245,\"ly\":92,\"mx\":243,\"my\":91},{\"lx\":247,\"ly\":92,\"mx\":245,\"my\":92},{\"lx\":248,\"ly\":92,\"mx\":247,\"my\":92},{\"lx\":251,\"ly\":92,\"mx\":248,\"my\":92},{\"lx\":254,\"ly\":92,\"mx\":251,\"my\":92},{\"lx\":257,\"ly\":92,\"mx\":254,\"my\":92},{\"lx\":260,\"ly\":92,\"mx\":257,\"my\":92},{\"lx\":263,\"ly\":92,\"mx\":260,\"my\":92},{\"lx\":267,\"ly\":92,\"mx\":263,\"my\":92},{\"lx\":270,\"ly\":90,\"mx\":267,\"my\":92},{\"lx\":273,\"ly\":89,\"mx\":270,\"my\":90},{\"lx\":276,\"ly\":87,\"mx\":273,\"my\":89},{\"lx\":278,\"ly\":86,\"mx\":276,\"my\":87},{\"lx\":280,\"ly\":83,\"mx\":278,\"my\":86},{\"lx\":282,\"ly\":81,\"mx\":280,\"my\":83},{\"lx\":283,\"ly\":79,\"mx\":282,\"my\":81},{\"lx\":285,\"ly\":76,\"mx\":283,\"my\":79},{\"lx\":286,\"ly\":75,\"mx\":285,\"my\":76},{\"lx\":286,\"ly\":73,\"mx\":286,\"my\":75},{\"lx\":287,\"ly\":71,\"mx\":286,\"my\":73},{\"lx\":288,\"ly\":71,\"mx\":287,\"my\":71},{\"lx\":288,\"ly\":70,\"mx\":288,\"my\":71},{\"lx\":288,\"ly\":69,\"mx\":288,\"my\":70}]"
            };

            List<Signature> signatures = new List<Signature>
            {
                signature1,
                signature1
            };

            // add signatures to document
            client.addContentToDocument(res, signatures);
        }

        [Fact]
        public void GetFieldsFromDocument()
        {
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            DocumentFields fileds = client.getDocumentFields("c0880b21-5c1d-4a15-87b0-1e61b832a5f6");
        }

        [Fact]
        public void SendSignatureRequest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            // add document from file and one signer
            Document document = new Document("../../../document.pdf");
            document.Signers.Add(new Signer("signer_1@example.com"));
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            SignatureRequest signatureRequestResponse = client.sendSignatureRequest(signatureRequest);

            // validate signature request response
            //validateResponse(signatureRequest, signatureRequestResponse, true);



            // get and validate signature request from database
            string signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.getSignatureRequest(signatureRequestId);

            validateSignatureRequest(signatureRequest, createdSignatureRequest, true);
        }

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
        public void testSendSignatureRequest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            // add document with possible attributes
            Document document = new Document("../../../document.pdf");
            document.Title = "Sample title";
            document.Subject = "Sample subject";
            document.Message = "Sample message";

            Signer signer1 = new Signer("signer_1@example.com");
            signer1.Order = 1;

            Signer signer2 = new Signer("signer_2@example.com");
            signer2.Order = 2;

            document.Signers.Add(signer1);
            document.Signers.Add(signer2);

            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            SignatureRequest signatureRequestResponse = client.sendSignatureRequest(signatureRequest);

            // validate signature request response
            validateResponse(signatureRequest, signatureRequestResponse, true);

            // get and validate signature request from database
            String signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.getSignatureRequest(signatureRequestId);

            Assert.Equal(2, createdSignatureRequest.Documents[0].Signers.Count);
            validateSignatureRequest(signatureRequest, createdSignatureRequest, true);
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
        public void testSendSignatureRequestWithExistingFields()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document("../../../document.pdf");
            document.ID = "e2d19bca-28a5-4eb4-83e2-60603bd7bf11";
            Signer signer = new Signer("signer_1@example.com");
            signer.Role = "Signer 1";
            // add fields
            ExistingField field1 = new ExistingField("b7f9bf0d-c616-4d9c-897f-3682b62e8f7d");
            field1.Content = "Sample content 1";
            field1.Required = true;
            signer.ExistingFields.Add(field1);

            // add second field to signer
            ExistingField field2 = new ExistingField("cc6a2a9c-54b4-43ac-9778-93192b2ab158");
            field2.Content = "Sample content 2";
            field2.Required = true;
            signer.ExistingFields.Add(field2);

            document.Signers.Add(signer);
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            SignatureRequest signatureRequestResponse = client.sendSignatureRequest(signatureRequest);

            // validate signature request response
            validateResponse(signatureRequest, signatureRequestResponse, false);

            // get and validate signature request from database
            SignatureRequest createdSignatureRequest = client.getSignatureRequest(
                    signatureRequestResponse.SignatureRequestId);
            validateSignatureRequest(signatureRequest, createdSignatureRequest, false);

            // get and validate fields from database
            Document expectedDocument = signatureRequest.Documents[0];
            DocumentFields documentFields = client.getDocumentFields(createdSignatureRequest.Documents[0].ID);
            validateDocumentFields(expectedDocument, documentFields);
        }

        /*
         * Test sending signature request for template.
         * Curl example:
         * {"documents" : [
         * {"document_id": "6586b79c-60a9-4986-a96d-2b8841cfb567",
         * "title": "Sample title", "subject": "Sample subject", "message": "Sample message",
         * "signers": [{"email": "signer_1@example.com"},{"email": "signer_2@example.com"}]}]}
         */
         [Fact]
         public void testSendSignatureRequestForTemplate()
         {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document();
            document.ID = "fb37a1c7-beb3-42dd-9d97-5733ef12a1ee";
            document.Title = "Sample title";
            document.Subject = "Sample subject";
            document.Message = "Sample message";

            Signer signer1 = new Signer("signer_1@example.com");
            signer1.Order = 1;

            Signer signer2 = new Signer("signer_2@example.com");
            signer2.Order = 2;

            document.Signers.Add(signer1);
            document.Signers.Add(signer2);
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            SignatureRequest signatureRequestResponse = client.sendSignatureRequest(signatureRequest);

            // validate signature request response
            validateResponse(signatureRequest, signatureRequestResponse, false);

            // get and validate signature request from database
            String signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.getSignatureRequest(signatureRequestId);

            validateSignatureRequest(signatureRequest, createdSignatureRequest, false);
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
         public void testSendSignatureRequestWithExistingFieldsForTemplate()
         {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            Document document = new Document("../../../document.pdf");
            document.ID = "c0880b21-5c1d-4a15-87b0-1e61b832a5f6";
            document.Title = "Sample title";
            document.Subject = "Sample subject";
            document.Message = "Sample message";

            // add first signer
            Signer signer1 = new Signer("signer_1@example.com");
            signer1.Role = "Employee";
            signer1.Order = 1;

            // add field for first signer
            ExistingField field1 = new ExistingField("f2eb6940-1797-4fef-ae7d-39bb76cbb2a7");
            field1.Content = "Sample content 1";
            field1.Label = "Please sign";
            field1.Required = true;
            field1.ReadOnly = false;
            signer1.ExistingFields.Add(field1);

            // add second field to first signer
            ExistingField field2 = new ExistingField("8e76a737-84ed-4cfa-959f-3500c7490de5");
            field2.Content = "James Williams";
            field2.Label = "Your name";
            field2.Required = true;
            field2.ReadOnly = false;
            signer1.ExistingFields.Add(field2);

            // add second signer
            Signer signer2 = new Signer("signer_2@example.com");
            signer2.Role = "Manager";
            signer2.Order = 2;

            // add field for second signer
            ExistingField field3 = new ExistingField("585a5230-eb43-4574-b8cd-0300249041de");
            field3.Content = "Mary Brown";
            field3.Label = "Please sign";
            field3.Required = true;
            field3.ReadOnly = false;
            signer2.ExistingFields.Add(field3);

            // add second field to second signer
            ExistingField field4 = new ExistingField("5fa4b24b-2775-4276-a1c5-8a6b0af58931");
            field4.Content = "Mary Brown";
            field4.Label = "Your name";
            field4.Required = false;
            field4.ReadOnly = false;
            signer2.ExistingFields.Add(field4);

            document.Signers.Add(signer1);
            document.Signers.Add(signer2);
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient("fba19cdd-a21c-46cc-90fc-28a77e2271a4");
            SignatureRequest signatureRequestResponse = client.sendSignatureRequest(signatureRequest);

            // validate signature request response
            validateResponse(signatureRequest, signatureRequestResponse, false);

            // get and validate signature request from database
            SignatureRequest createdSignatureRequest = client.getSignatureRequest(
                    signatureRequestResponse.SignatureRequestId);
            validateSignatureRequest(signatureRequest, createdSignatureRequest, false);

            // get and validate fields from database
            Document expectedDocument = signatureRequest.Documents[0];
            DocumentFields documentFields = client.getDocumentFields(createdSignatureRequest.Documents[0].ID);
            validateDocumentFields(expectedDocument, documentFields);
        }

        private void validateDocumentFields(Document document, DocumentFields documentFields)
        {
            foreach (Signer signer in document.Signers)
            {
                foreach (Field field in signer.Fields)
                {
                    // assert that all fields from all signers in document (document.getSigners()) can be found
                    // in documentFields.getDocumentFields()
                    DocumentField documentField = findDocumentField(field.ApiId, documentFields);
                    Assert.NotNull(documentField);
                    // and all their attributes are equal
                    Assert.Equal(field.Page, documentField.Page);
                    Assert.Equal(field.Type, documentField.Type);
                    Assert.Equal(field.Label, documentField.Label);
                    Assert.Equal(field.Required, documentField.Required);
                    Assert.Equal(field.Name, documentField.Name);
                    Assert.Equal(field.ReadOnly, documentField.ReadOnly);
                    Assert.Equal(field.Content, documentField.Content);
                }

                foreach (ExistingField field in signer.ExistingFields)
                {
                    DocumentField documentField = findDocumentField(field.ApiId, documentFields);
                    Assert.NotNull(documentField);
                    // and all their attributes are equal
                    Assert.Equal(field.Label, documentField.Label);
                    Assert.Equal(field.Required, documentField.Required);
                    Assert.Equal(field.ReadOnly, documentField.ReadOnly);
                    Assert.Equal(field.Content, documentField.Content);
                }
            }
        }

        private DocumentField findDocumentField(String apiId, DocumentFields documentFields)
        {
            foreach (DocumentField documentField in documentFields.Fileds)
            {
                if (documentField.apiId.Equals(apiId))
                {
                    return documentField;
                }
            }
            return null;
        }

        private void validateResponse(SignatureRequest expected, SignatureRequest actual, bool isDocument)
        {
            int i = 0;
            foreach (Document document in actual.Documents)
            {

                foreach (Signer signer in document.Signers)
                {
                    string signDocumentUrl = signer.SignDocumentUrl;
                    Assert.NotNull(signDocumentUrl);
                    // validate signDocumentUrl
                    Regex regex = new Regex("(?=.*documentId=)(?=.*invitationId=).*$");
                    Assert.Matches(regex, signDocumentUrl);

                    // template has different ID for expected SignatureRequest.
                    if (isDocument)
                    {
                        string expectedDocumentId = expected.Documents[i].ID;
                        signDocumentUrl.Contains(expectedDocumentId);
                    }
                }
                ++i;
            }
        }

        private Signer getSignerByEmail(SignatureRequest signatureRequest, string email)
        {
            foreach (Document document in signatureRequest.Documents)
            {
                foreach (Signer signer in document.Signers)
                {
                    if (signer.Email.Equals(email))
                    {
                        return signer;
                    }
                }
            }
            return null;
        }

        protected void validateSignatureRequest(SignatureRequest expected, SignatureRequest actual, bool isDocument)
        {

            // assert all high level attributes are the same: "send_emails", "embedded" etc.
            Assert.Equal<bool>(expected.SendEmails, actual.SendEmails);
            Assert.Equal(expected.RedirectAfterSigningToUrl, actual.RedirectAfterSigningToUrl);
            Assert.Equal(expected.RedirectForSigningToUrl, actual.RedirectForSigningToUrl);
            Assert.Equal(expected.Embedded, actual.Embedded);
            Assert.Equal(expected.UseTextTags, actual.UseTextTags);
            Assert.Equal(expected.HideTextTags, actual.HideTextTags);
            Assert.Equal(expected.Completed, actual.Completed);
            
            // iterate over documents and assert all their attributes are the same
            for (int i = 0; i < expected.Documents.Count; ++i)
            {
                Document expectedDocument = expected.Documents[i];
                Document actualDocument = actual.Documents[i];
                // template has different ID for expected SignatureRequest.
                if (isDocument)
                {
                    Assert.Equal(expectedDocument.ID, actualDocument.ID);
                }
                // check document title; if not set - generated
                if (expectedDocument.Title == null)
                {
                    Assert.NotNull(actualDocument.Title);
                }
                else
                {
                    Assert.Equal(expectedDocument.Title, actualDocument.Title);
                }

                // check document subject and message; if not set - taken by default
                if (expectedDocument.Subject == null)
                {
                    Assert.NotNull(actualDocument.Subject);
                }
                else
                {
                    Assert.Equal(expectedDocument.Subject, actualDocument.Subject);
                }

                if (expectedDocument.Message == null)
                {
                    Assert.NotNull(actualDocument.Subject);
                }
                else
                {
                    Assert.Equal(expectedDocument.Message, actualDocument.Message);
                }

                // for each document iterate over signers and assert all their attributes are the same
                for (int s = 0; s < expectedDocument.Signers.Count; s++)
                {
                    Signer expectedSigner = expectedDocument.Signers[s];
                    Signer actualSigner = getSignerByEmail(actual, expectedSigner.Email);

                    Assert.Equal(expectedSigner.AccessCode, actualSigner.AccessCode);
                    Assert.Equal(expectedSigner.Email, actualSigner.Email);
                    Assert.Equal(expectedSigner.Order, actualSigner.Order);
                    // validate if role defined for signer
                    if (expectedSigner.Role != null)
                    {
                        Assert.Equal(expectedSigner.Role, actualSigner.Role);
                    }
                    // validate signDocumentUrl
                    Assert.NotNull(actualSigner.SignDocumentUrl);
                    Assert.Matches(new Regex("(?=.*documentId=)(?=.*invitationId=).*$"), actualSigner.SignDocumentUrl);
                    Assert.Equal(expectedSigner.SignatureCompleted, actualSigner.SignatureCompleted);
                }
            }
        }
    }
}
