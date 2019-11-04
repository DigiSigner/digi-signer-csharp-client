
using Xunit;

namespace DigiSigner.Client.Tests
{
    public class SignatureRequestSimpleTests : DigiSignerClientTestsHelper
    {
        /*
         * Tests simple send signature request.
         * Curl example:
         * {"documents" : [
         * {"document_id": "06c4d320-d6c5-492b-b343-8482338ef9d0",
         * "signers": [*{"email": "signer_1@example.com"}]}]}
         */
        [Fact]
        public void SendSignatureRequestTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            // add document from file and one signer
            Document document = new Document(relativePathToFileOfTheDocument);
            document.Signers.Add(new Signer(signers["signer1"]["email"]));
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, true);

            // get and validate signature request from database
            string signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(signatureRequestId);

            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, true);
        }

        /*
         * Tests simple send signature request with branding info.
         * Curl example:
         * {"branding": {
         * "reply_to_email": "peter.rogers@digisigner.com",
         * "email_from_field": "Your company"
         * },
         * {"documents" : [
         * {"document_id": "06c4d320-d6c5-492b-b343-8482338ef9d0",
         * "signers": [*{"email": "signer_1@example.com"}]}],
         }
         */
        [Fact]
        public void SendSignatureRequestWithBrandingTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            // add document from file and one signer
            Document document = new Document(relativePathToFileOfTheDocument);
            document.Signers.Add(new Signer(signers["signer1"]["email"]));
            signatureRequest.Documents.Add(document);

            // add branding info
            signatureRequest.Branding.EmailFromField = "Your company";
            signatureRequest.Branding.ReplyToEmail = "peter.rogers@digisigner.com";

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, true);

            // get and validate signature request from database
            string signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(signatureRequestId);

            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, true);
        }

        /*
         * Test to send signature request with fields.
         * Example string for curl:
         * {"documents" : [
         *   {"document_id": "d083448d-ad0e-4742-af19-cfc1117445b1",
         *    "signers": [{"email": "signer_1@example.com",
         *         "fields": [{"page": 0, "rectangle": [100, 100, 300, 200], "type": "SIGNATURE"},
         *                    {"page": 0, "rectangle": [350, 100, 400, 200], *"type": "TEXT"}]}]}]}
         */
        [Fact]
        public void SendSignatureRequestWithFieldsTest()
        {
            // build signature request
            SignatureRequest signatureRequest = new SignatureRequest();
            signatureRequest.SendEmails = false;

            // add document from file and one signer
            Document document = new Document(relativePathToFileOfTheDocument);
            Signer signer = new Signer(signers["signer1"]["email"]);
            // add field with API ID to be able find it latter
            Field field1 = new Field(0, new int[] { 100, 100, 300, 200 }, FieldType.SIGNATURE);
            field1.ApiId = "Fild1ApiId";

            signer.Fields.Add(field1);
            Field field2 = new Field(0, new int[] { 350, 100, 400, 220 }, FieldType.TEXT);
            field2.ApiId = "Fild2ApiId";
            signer.Fields.Add(field2);

            document.Signers.Add(signer);
            signatureRequest.Documents.Add(document);

            // execute signature request
            DigiSignerClient client = new DigiSignerClient(apiId);
            SignatureRequest signatureRequestResponse = client.SendSignatureRequest(signatureRequest);

            // validate signature request response
            ValidateResponse(signatureRequest, signatureRequestResponse, true);

            // get and validate signature request from database
            string signatureRequestId = signatureRequestResponse.SignatureRequestId;
            SignatureRequest createdSignatureRequest = client.GetSignatureRequest(signatureRequestId);

            ValidateSignatureRequest(signatureRequest, createdSignatureRequest, true);

            // get and validate fields from database
            Document expectedDocument = signatureRequest.Documents[0];
            DocumentFields documentFields = client.GetDocumentFields(createdSignatureRequest.Documents[0].ID);
            ValidateDocumentFields(expectedDocument, documentFields);
        }        
    }
}
