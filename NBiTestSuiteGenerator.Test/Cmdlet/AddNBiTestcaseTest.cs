using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiTestcase")]
    public class AddNBiTestcaseTest
    {
        #region FIELDS
        private Runspace _runspace;
        private static readonly string _assemblyPath = typeof(AddNBiTestcase).Assembly.Location;
        private static readonly string _fileDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly string _testCasePath = $"{ _fileDirectory }TestFiles\\AddNBiTestcase.nbitx";
        private static readonly string _templatePath = $"{ _fileDirectory }TestFiles\\AddNBiTestcase.nbitt";
        private static readonly string _csvPath      = $"{ _fileDirectory }TestFiles\\AddNBiTestcase.csv";
        private static readonly string _queryPath    = $"{ _fileDirectory }TestFiles\\AddNBiTestcase.sql";

        private static readonly string _testCaseString = File.ReadAllText(_testCasePath).Replace("'", "''");
        private static readonly string _templateString = File.ReadAllText(_templatePath).Replace("'", "''");
        private static readonly string _queryString = File.ReadAllText(_queryPath).Replace("'", "''");

        private static readonly string _connectionString = "Data Source=(local)\\sql2017;Initial Catalog=tempdb;Integrated Security=SSPI"; // TODO

        #endregion FIELDS

        #region METHODS

        #region Helpers

        public static TestCaseData[] GetTestCases()
        {
            return new TestCaseData[]
            {
                // Static test cases
                new TestCaseData($"-TestCase '{ _testCaseString }'")
                    .SetName("AddNBiTestcase_TestCaseString"),
                new TestCaseData($"-TestCaseFilepath '{ _testCasePath }'")
                    .SetName("AddNBiTestcase_TestCaseFile"),

                // From CSV
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateName 'ExistsDimension'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromCsv"),
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateFilepath '{ _templatePath }'")
                    .SetName("AddNBiTestcase_TemplateFileFromCsv"),
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateString '{ _templateString }'")
                    .SetName("AddNBiTestcase_TemplateStringFromCsv"),

                // From query
                new TestCaseData($"-Query '{ _queryString }' -ConnectionString '{ _connectionString }' -TemplateName 'ExistsDimension'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromQuery"),
                new TestCaseData($"-Query '{ _queryString }' -ConnectionString '{ _connectionString }' -TemplateFilepath '{ _templatePath }'")
                    .SetName("AddNBiTestcase_TemplateFileFromQuery"),
                new TestCaseData($"-Query '{ _queryString }' -ConnectionString '{ _connectionString }' -TemplateString '{ _templateString }'")
                    .SetName("AddNBiTestcase_TemplateStringFromQuery"),

                // From query file
                new TestCaseData($"-QueryFilepath '{ _queryPath }' -ConnectionString '{ _connectionString }' -TemplateName 'ExistsDimension'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromQueryFile"),
                new TestCaseData($"-QueryFilepath '{ _queryPath }' -ConnectionString '{ _connectionString }' -TemplateFilepath '{ _templatePath }'")
                    .SetName("AddNBiTestcase_TemplateFileFromQueryFile"),
                new TestCaseData($"-QueryFilepath '{ _queryPath }' -ConnectionString '{ _connectionString }' -TemplateString '{ _templateString }'")
                    .SetName("AddNBiTestcase_TemplateStringFromQueryFile"),

                // Group
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateName 'ExistsDimension' -GroupPath 'grp1'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromCsvWithStaticGroupOneLevel"),
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateName 'ExistsDimension' -GroupPath 'grp1|grp2'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromCsvWithStaticGroupTwoLevels"),
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateName 'ExistsDimension' -GroupPath '$dimension$'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromCsvWithDynamicGroupOneLevel"),
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateName 'ExistsDimension' -GroupPath '$perspective$|$dimension$'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromCsvWithDynamicGroupTwoLevels"),
                
                // Scope
                new TestCaseData($"-CsvFilepath '{ _csvPath }' -TemplateName 'ExistsDimension' -CaseScope 'ScopeA'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromCsvWithScope"),

                // From DataTable
                new TestCaseData($"-Datatable $dataTable -TemplateName 'ExistsDimension'")
                    .SetName("AddNBiTestcase_TemplateEmbeddedFromTable"),
                new TestCaseData($"-Datatable $dataTable -TemplateFilepath '{ _templatePath }'")
                    .SetName("AddNBiTestcase_TemplateFileFromTable"),
                new TestCaseData($"-Datatable $dataTable -TemplateString '{ _templateString }'")
                    .SetName("AddNBiTestcase_TemplateStringFromTable"),

                // Ad hoc

            };
        }

        public static string GetScript(string parameters)
        {
            string scriptPattern =
@"
$dataTable = [System.Data.DataTable]('dimensions')
$dataTable.Columns.Add([System.Data.DataColumn]@{{ColumnName = 'dimension'}})
$dataTable.Columns.Add([System.Data.DataColumn]@{{ColumnName = 'perspective'}})
[void] $dataTable.Rows.Add('DimA','PerspectiveX')
[void] $dataTable.Rows.Add('DimB','PerspectiveX')
[void] $dataTable.Rows.Add('DimA','PerspectiveY')

Import-Module {0}
$testsuite = New-NBiTestSuite
Add-NBiTestcase -TestSuite $testSuite {1}"; // 0: Assembly path, 1: Parameters

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

        public void AddNBiTestcase_(string parameters)
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
