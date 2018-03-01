using System;
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
    }
}
