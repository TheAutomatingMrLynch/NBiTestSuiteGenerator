using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiTestSuite")]
    public class ImportNBiTestSuiteTest
    {
        #region FIELDS
        private Runspace _runspace;
        private static readonly string _assemblyPath = typeof(ImportNBiTestSuite).Assembly.Location;
        private static readonly string _fileDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _filepath = $"{ _fileDirectory }TestFiles\\TestSuite.nbits";
        #endregion FIELDS

        #region METHODS

        #region Helpers

        public static TestCaseData[] GetTestCases()
        {
            return new TestCaseData[]
            {
                new TestCaseData($"-SourceFilepath '{ _filepath }'")
                    .SetName("ImportNBiTestSuite_NoGroup"),
                new TestCaseData($"-SourceFilepath '{ _filepath }' -GroupPath ''")
                    .SetName("ImportNBiTestSuite_BlankGroup"),
                new TestCaseData($"-SourceFilepath '{ _filepath }' -GroupPath 'grp1'")
                    .SetName("ImportNBiTestSuite_Group")
            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
$testsuite = New-NBiTestSuite
Import-NBiTestSuite `
    -TestSuite $testSuite `
    {1}"; // 0: Assembly path, 1: Parameters

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
        public void ImportNBiTestSuite_(string parameters)
        {
            // Arrange
            string script = GetScript(parameters);
            Pipeline pipeline = _runspace.CreatePipeline(script);

            // Act
            var result = pipeline.Invoke();
            GenerationState testSuite = (GenerationState)result.FirstOrDefault().BaseObject;
            string[] actual = testSuite.Suite.Children.Select(c => c.Name).ToArray();

            // Assert
            CollectionAssert.IsNotEmpty(actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}