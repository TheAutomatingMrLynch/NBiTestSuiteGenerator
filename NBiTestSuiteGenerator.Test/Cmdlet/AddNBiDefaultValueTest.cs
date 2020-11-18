using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiDefaultValue")]
    public class AddNBiDefaultValueTest
    {
        #region FIELDS
        private Runspace _runspace;
        private static readonly string _assemblyPath = typeof(AddNBiDefaultValue).Assembly.Location;
        #endregion FIELDS

        #region METHODS

        #region Helpers

        public static TestCaseData[] GetTestCases()
        {
            return new TestCaseData[]
            {
                new TestCaseData($"-Variable 'ConnectionString' -Value 'BlaBla'")
                    .SetName("AddNBiDefaultValue_NoScope"),
                new TestCaseData($"-Variable 'ConnectionString' -Value 'BlaBla' -Scope 'Everywhere'")
                    .SetName("AddNBiDefaultValue_ScopeEverywhere"),
                new TestCaseData($"-Variable 'ConnectionString' -Value 'BlaBla' -Scope 'SystemUnderTest'")
                    .SetName("AddNBiDefaultValue_ScopeSystemUnderTest"),
                new TestCaseData($"-Variable 'ConnectionString' -Value 'BlaBla' -Scope 'Assert'")
                    .SetName("AddNBiDefaultValue_ScopeAssert"),
                new TestCaseData($"-Variable 'ConnectionString' -Value 'BlaBla' -Scope 'SetupCleanup'")
                    .SetName("AddNBiDefaultValue_ScopeSetupCleanup")
            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
$testsuite = New-NBiTestSuite
Add-NBiDefaultValue -TestSuite $testSuite {1}"; // 0: Assembly path, 1: Parameters

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
        public void AddNBiDefaultValue_(string parameters)
        {
            // Arrange
            string script = GetScript(parameters);
            Pipeline pipeline = _runspace.CreatePipeline(script);

            // Act
            var result = pipeline.Invoke();
            GenerationState testSuite = (GenerationState)result.FirstOrDefault().BaseObject;
            var actual = testSuite.Settings.Defaults.Select(d => d.ConnectionString).Select(c => c.Inline).ToArray();

            // Assert

            CollectionAssert.IsNotEmpty(actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}