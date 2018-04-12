using DigiSigner.Client;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace DigiSigner.Client.Tests
{
    public class DigiSignerClientTestsHelper
    {
        protected const string apiId = "fba19cdd-a21c-46cc-90fc-28a77e2271a4";
        protected const string relativePathToFileOfTheDocument = "../../../document.pdf";
        protected const string relativePathToDownloadDocument = "../../../2.pdf";

        protected const string idOfTheDocumentWithFileds = "c0880b21-5c1d-4a15-87b0-1e61b832a5f6";
        protected const string idOfTheDocumentWithExisitingFileds = "e2d19bca-28a5-4eb4-83e2-60603bd7bf11";
        protected const string idOfTheTemplate = "fb37a1c7-beb3-42dd-9d97-5733ef12a1ee";

        protected Dictionary<string, string> documentValues = new Dictionary<string, string>
        {
            {"title", "Sample title" },
            {"subject", "Sample subject"},
            {"message", "Sample message"}
        };

        protected Dictionary<string, Dictionary<string, string> > signers = new Dictionary<string, Dictionary<string, string>>
        {
            { "signer1", new Dictionary<string, string>{
                { "email", "signer_1@example.com" } }
            },
            { "signer2", new Dictionary<string, string>{
                { "email", "signer_2@example.com" } }
            }
        };


        protected void ValidateSignatureRequest(SignatureRequest expected, SignatureRequest actual, bool isDocument)
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
                    Signer actualSigner = GetSignerByEmail(actual, expectedSigner.Email);

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
        protected void ValidateDocumentFields(Document document, DocumentFields documentFields)
        {
            foreach (Signer signer in document.Signers)
            {
                foreach (Field field in signer.Fields)
                {
                    // assert that all fields from all signers in document (document.getSigners()) can be found
                    // in documentFields.getDocumentFields()
                    DocumentField documentField = FindDocumentField(field.ApiId, documentFields);
                    Assert.NotNull(documentField);
                    // and all their attributes are equal
                    Assert.Equal(field.Page, documentField.Page);
                    Assert.Equal(field.Type, documentField.Type);
                    Assert.Equal(field.Label, documentField.Label);
                    Assert.Equal(field.Required, documentField.Required);
                    Assert.Equal(field.GroupId, documentField.GroupId);
                    Assert.Equal(field.ReadOnly, documentField.ReadOnly);
                    Assert.Equal(field.Content, documentField.Content);
                }

                foreach (ExistingField field in signer.ExistingFields)
                {
                    DocumentField documentField = FindDocumentField(field.ApiId, documentFields);
                    Assert.NotNull(documentField);
                    // and all their attributes are equal
                    Assert.Equal(field.Label, documentField.Label);
                    Assert.Equal(field.Required, documentField.Required);
                    Assert.Equal(field.ReadOnly, documentField.ReadOnly);
                    Assert.Equal(field.Content, documentField.Content);
                }
            }
        }

        protected DocumentField FindDocumentField(String apiId, DocumentFields documentFields)
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

        protected void ValidateResponse(SignatureRequest expected, SignatureRequest actual, bool isDocument)
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

        protected Signer GetSignerByEmail(SignatureRequest signatureRequest, string email)
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
    }
}
