using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiCsvProfile")]
    public class NewNBiCsvPRofileTest
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
                new TestCaseData("-FieldSeparator 'a'", "FieldSeparator", 'a')
                    .SetName("NewNBiCsvProfile_FieldSeparator"),
                new TestCaseData("-TextQualifier 'a'", "TextQualifier", 'a')
                    .SetName("NewNBiCsvProfile_TextQualifier"),
                new TestCaseData("-RecordSeparator 'aaa'", "RecordSeparator", "aaa")
                    .SetName("NewNBiCsvProfile_RecordSeparator"),
                new TestCaseData("-FirstRowHeader", "FirstRowHeader", true)
                    .SetName("NewNBiCsvProfile_FirstRowHeader"),
                new TestCaseData("-FirstRowHeader:$true", "FirstRowHeader", true)
                    .SetName("NewNBiCsvProfile_FirstRowHeaderValueTrue"),
                new TestCaseData("-FirstRowHeader:$false", "FirstRowHeader", false)
                    .SetName("NewNBiCsvProfile_FirstRowHeaderValueFalse"),
                new TestCaseData("-EmptyCell 'aaa'", "EmptyCell", "aaa")
                    .SetName("NewNBiCsvProfile_EmptyCell"),
                new TestCaseData("-MissingCell 'aaa'", "MissingCell", "aaa")
                    .SetName("NewNBiCsvProfile_MissingCell")
            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
New-NBiCsvProfile {1}"; // 0: Assembly path, 1: Parameters

            return String.Format(
                    scriptPattern,
                    _assemblyPath,
                    parameters
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
        public void GetNBiCsvProfile_(string parameters, string propertyName, object expected)
        {
            // Arrange
            string script = GetScript(parameters);
            Pipeline pipeline = _runspace.CreatePipeline(script);
            
            // Act
            var result = pipeline.Invoke();
            CsvProfileXml profile = (CsvProfileXml)result.FirstOrDefault().BaseObject;
            var actual = typeof(CsvProfileXml).GetProperty(propertyName).GetValue(profile);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}