using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiCsvProfile")]
    public class SaveNBiCsvProfileTest
    {
        #region FIELDS
        private Runspace _runspace;
        private static readonly string _assemblyPath = typeof(AddNBiReferenceValue).Assembly.Location;
        #endregion FIELDS

        #region METHODS

        #region Helpers
        public static TestCaseData[] GetTestCases()
        {
            return new TestCaseData[]
            {
                new TestCaseData("$profile.FieldSeparator = 'a'", "/d:CsvProfileXml/@field-separator", "a")
                    .SetName("SaveNBiCsvProfile_FieldSeparator"),
                new TestCaseData("$profile.TextQualifier = 'a'", "/d:CsvProfileXml/@text-qualifier", "a")
                    .SetName("SaveNBiCsvProfile_TextQualifier"),
                new TestCaseData("$profile.RecordSeparator = 'aaa'", "/d:CsvProfileXml/@record-separator", "aaa")
                    .SetName("SaveNBiCsvProfile_RecordSeparator"),
                new TestCaseData("$profile.FirstRowHeader = $true", "/d:CsvProfileXml/@first-row-header", "true")
                    .SetName("SaveNBiCsvProfile_FirstRowHeader"),
                new TestCaseData("$profile.EmptyCell = 'aaa'", "/d:CsvProfileXml/@empty-cell", "aaa")
                    .SetName("SaveNBiCsvProfile_EmptyCell"),
                new TestCaseData("$profile.MissingCell = 'aaa'", "/d:CsvProfileXml/@missing-cell", "aaa")
                    .SetName("SaveNBiCsvProfile_MissingCell")
            };
        }

        public static string GetScript(string parameters, string filePath)
        {
            string scriptPattern =
@"Import-Module {0}
$profile = New-Object NBi.Xml.Settings.CsvProfileXml
{1}
Save-NBiCsvProfile -CsvProfile $profile -FilePath {2}"; // 0: Assembly path, 1: Parameters, 2: File path

            return String.Format(
                    scriptPattern,
                    _assemblyPath,
                    parameters,
                    filePath
                );
        }

        
        #endregion Helpers

        #region Setup and teardown
        [OneTimeSetUp]
        public void Setup()
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _runspace.Close();
        }
        #endregion Setup and teardown

        #region Tests
        [Test]
        [TestCaseSource("GetTestCases")]
        public void SaveNBiCsvProfile_(string parameters, string xpath, object expected)
        {
            // Arrange
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveNBiCsvProfile_SavesFile.xml");
            if (File.Exists(filepath)) { File.Delete(filepath); }

            string script = GetScript(parameters, filepath);
            Pipeline pipeline = _runspace.CreatePipeline(script);

            try
            {
                // Act
                var result = pipeline.Invoke();

                // Assert
                List<XAttribute> xpathResult;
                using (FileStream stream = File.OpenRead(filepath))
                {
                    XDocument xdoc = XDocument.Load(stream);
                    XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
                    manager.AddNamespace("d", "");
                    xpathResult = ((IEnumerable)xdoc.Root.XPathEvaluate(xpath, manager)).Cast<XAttribute>().ToList();
                }
                var actual = xpathResult.FirstOrDefault();

                FileAssert.Exists(filepath);
                Assert.AreEqual(expected, actual.Value);
            }
            // Cleanup
            finally
            {
                if (File.Exists(filepath)) { File.Delete(filepath); }
            }
        }
        #endregion Tests

        #endregion METHODS
    }
}