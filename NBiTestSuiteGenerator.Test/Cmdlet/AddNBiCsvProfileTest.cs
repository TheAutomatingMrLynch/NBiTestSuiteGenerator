using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiCsvProfile")]
    public class AddNBiCsvProfileTest
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
                    .SetName("AddNBiCsvProfile_FieldSeparator"),
                new TestCaseData("-TextQualifier 'a'", "TextQualifier", 'a')
                    .SetName("AddNBiCsvProfile_TextQualifier"),
                new TestCaseData("-RecordSeparator 'aaa'", "RecordSeparator", "aaa")
                    .SetName("AddNBiCsvProfile_RecordSeparator"),
                new TestCaseData("-FirstRowHeader", "FirstRowHeader", true)
                    .SetName("AddNBiCsvProfile_FirstRowHeader"),
                new TestCaseData("-FirstRowHeader:$true", "FirstRowHeader", true)
                    .SetName("AddNBiCsvProfile_FirstRowHeaderValueTrue"),
                new TestCaseData("-FirstRowHeader:$false", "FirstRowHeader", false)
                    .SetName("AddNBiCsvProfile_FirstRowHeaderValueFalse"),
                new TestCaseData("-EmptyCell 'aaa'", "EmptyCell", "aaa")
                    .SetName("AddNBiCsvProfile_EmptyCell"),
                new TestCaseData("-MissingCell 'aaa'", "MissingCell", "aaa")
                    .SetName("AddNBiCsvProfile_MissingCell")
            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
$testsuite = New-NBiTestSuite
Add-NBiCsvProfile -TestSuite $testSuite {1}"; // 0: Assembly path, 1: Parameters

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
        public void AddNBiCsvProfile_(string parameters, string propertyName, object expected)
        {
            // Arrange
            string script = GetScript(parameters);
            Pipeline pipeline = _runspace.CreatePipeline(script);
            
            // Act
            var result = pipeline.Invoke();
            GenerationState testSuite = (GenerationState)result.FirstOrDefault().BaseObject;
            var actual = typeof(CsvProfileXml).GetProperty(propertyName).GetValue(testSuite.Settings.CsvProfile);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}