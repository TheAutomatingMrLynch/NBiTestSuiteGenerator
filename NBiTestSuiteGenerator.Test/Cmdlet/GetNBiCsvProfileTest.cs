﻿using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiCsvProfile")]
    public class GetNBiCsvProfileTest
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
                    .SetName("GetNBiCsvProfile_FieldSeparator"),
                new TestCaseData("-TextQualifier 'a'", "TextQualifier", 'a')
                    .SetName("GetNBiCsvProfile_TextQualifier"),
                new TestCaseData("-RecordSeparator 'aaa'", "RecordSeparator", "aaa")
                    .SetName("GetNBiCsvProfile_RecordSeparator"),
                new TestCaseData("-FirstRowHeader", "FirstRowHeader", true)
                    .SetName("GetNBiCsvProfile_FirstRowHeader"),
                new TestCaseData("-FirstRowHeader:$true", "FirstRowHeader", true)
                    .SetName("GetNBiCsvProfile_FirstRowHeaderValueTrue"),
                new TestCaseData("-FirstRowHeader:$false", "FirstRowHeader", false)
                    .SetName("GetNBiCsvProfile_FirstRowHeaderValueFalse"),
                new TestCaseData("-EmptyCell 'aaa'", "EmptyCell", "aaa")
                    .SetName("GetNBiCsvProfile_EmptyCell"),
                new TestCaseData("-MissingCell 'aaa'", "MissingCell", "aaa")
                    .SetName("GetNBiCsvProfile_MissingCell")
            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
$testsuite = New-NBiTestSuite
Add-NBiCsvProfile -TestSuite $testSuite {1} | Out-Null
Get-NBiCsvProfile -TestSuite $testSuite"; // 0: Assembly path, 1: Parameters

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
            var csvProfile = (CsvProfileXml)result.FirstOrDefault().BaseObject;
            var actual = typeof(CsvProfileXml).GetProperty(propertyName).GetValue(csvProfile);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}