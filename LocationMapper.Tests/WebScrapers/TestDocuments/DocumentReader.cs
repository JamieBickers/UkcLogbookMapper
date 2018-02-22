using System;
using System.Collections.Generic;
using System.Text;

using static System.IO.File;

namespace LocationMapper.Tests.WebScrapers.TestDocuments
{
    class DocumentReader
    {
        private const string BaseTestDocumentsPath = @"C:\Users\Jamie\Desktop\UkcLogbookMapper\LocationMapper.Tests\WebScrapers\TestDocuments\";

        public string ReadDocument(string fileName)
        {
            fileName = fileName.Replace('/', ' ');
            fileName = fileName.Replace('\\', ' ');

            return ReadAllText(BaseTestDocumentsPath + fileName);
        }
    }
}
