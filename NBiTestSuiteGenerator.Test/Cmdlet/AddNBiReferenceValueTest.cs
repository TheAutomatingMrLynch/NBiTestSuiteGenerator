using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiReferenceValue")]
    public class AddNBiReferenceValueTest
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
                new TestCaseData("-Name 'SourceSystem' -Variable 'ConnectionString' -Value 'BlaBla'")
                    .SetName("AddNBiReferenceValue_ConnectionString")
            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
$testsuite = New-NBiTestSuite
Add-NBiReferenceValue -TestSuite $testSuite {1}"; // 0: Assembly path, 1: Parameters

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
        public void AddNBiReferenceValue_(string parameters)
        {
            // Arrange
            string script = GetScript(parameters);
            Pipeline pipeline = _runspace.CreatePipeline(script);
            
            // Act
            var result = pipeline.Invoke();
            GenerationState testSuite = (GenerationState)result.FirstOrDefault().BaseObject;
            var actual = testSuite.Settings.References.Select(d => d.ConnectionString).Select(c => c.Inline).ToArray();

            // Assert
            
            CollectionAssert.IsNotEmpty(actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}