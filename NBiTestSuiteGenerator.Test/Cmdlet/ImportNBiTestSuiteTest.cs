using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiTestSuite")]
    public class ImportNBiTestSuiteTest : CmdletTestBase
    {
        #region FIELDS
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

        public static string GetScript(string assemblyPath, string parameters)
        {
            string scriptPattern =
@"Import-Module {0}
$testsuite = New-NBiTestSuite
Import-NBiTestSuite `
    -TestSuite $testSuite `
    {1}"; // 0: Assembly path, 1: Parameters

            return String.Format(
                    scriptPattern,
                    assemblyPath,
                    parameters
                );
        }

        #endregion Helpers

        #region Tests
        [Test]
        [TestCaseSource("GetTestCases")]
        public void ImportNBiTestSuite_(string parameters)
        {
            // Arrange
            string script = GetScript(AssemblyPath, parameters);
            Pipeline pipeline = Runspace.CreatePipeline(script);

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