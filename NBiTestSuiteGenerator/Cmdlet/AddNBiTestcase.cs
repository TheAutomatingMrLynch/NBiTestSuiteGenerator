using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Action.Template;
using System;
using System.Data;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Adds a set of test cases to an NBi test suite. </para>
    /// <para type="description">Adds a set of test cases to an NBi test suite. </para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "NBiTestCase", DefaultParameterSetName = "FromCsv")]
    [OutputType(typeof(TestSuite))]
    //[CmdletBinding()]
    public class AddNBiTestcase : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">Test suite to add test cases to. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public TestSuite TestSuite { get; set; }

        /// <summary>
        /// <para type="description">String containing an NBi test case where &gt;test&lt; is the root node. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TestCaseString-Static")]
        public string TestCase { get; set; }

        /// <summary>
        /// <para type="description">Filepath to a .nbitx file containing an NBi test case where &gt;test&lt; is the root node. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TestCaseFile-Static")]
        public string TestCaseFilepath { get; set; }

        /// <summary>
        /// <para type="description">Filepath to a CSV file containing test case variables. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromCsv")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromCsv")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromCsv")]
        public string CsvFilepath { get; set; }

        /// <summary>
        /// <para type="description">Query returning test case variables. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromQuery")]
        public string Query { get; set; }

        /// <summary>
        /// <para type="description">Filepath to a file containing a query returning test case variables. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromQueryFile")]
        public string QueryFilepath { get; set; }

        /// <summary>
        /// <para type="description">Connection string a query or query file uses to get test case variables. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromQueryFile")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// <para type="description">Data table containing test case variables. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromTable")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromTable")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromTable")]
        public DataTable Datatable { get; set; }

        /// <summary>
        /// <para type="description">Name of an embedded test case template. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromCsv")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateEmbedded-FromTable")]
        public EmbeddedTemplate.Template TemplateName { get; set; }

        /// <summary>
        /// <para type="description">Filepath to an .nbitt file containing a test case template. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromCsv")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateFile-FromTable")]
        public string TemplateFilepath { get; set; }

        /// <summary>
        /// <para type="description">Test case template. </para>
        /// </summary>
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromCsv")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromQuery")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromQueryFile")]
        [Parameter(Mandatory = true, ParameterSetName = "TemplateString-FromTable")]
        public string TemplateString { get; set; }

        /// <summary>
        /// <para type="description">Optional test case scope to add new test cases to. Used to reference added test cases. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string CaseScope { get; set; }

        /// <summary>
        /// <para type="description">Optional grouping path of tests. Separate each level using a pipe character (|). Path can be both static (e.g. 'grp1|grp2') and dynamic (e.g. '$table$|$column$') or a combination. )</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string GroupPath { get; set; }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            if (!String.IsNullOrEmpty(CaseScope))
            {
                IAction scopeAction = new ScopeCaseAction(CaseScope);
                scopeAction.Execute(TestSuite);
                WriteVerbose(scopeAction.Display);
            }

            string templateActionName = ParameterSetName.Split(new[] { '-' }, 2)[0];
            string actionName         = ParameterSetName.Split(new[] { '-' }, 2)[1];

            if (actionName == "Static")
            {
                IAction testCaseAction = ProcessStaticAction(templateActionName);
                testCaseAction.Execute(TestSuite);
                WriteVerbose(testCaseAction.Display);
            }
            else
            {
                ProcessTemplate(templateActionName).Execute(TestSuite);
                ProcessCases(actionName).Execute(TestSuite);

                if (String.IsNullOrEmpty(GroupPath))
                {
                    IAction suiteAction = new GenerateTestSuiteAction(false);
                    suiteAction.Execute(TestSuite);
                    WriteVerbose(suiteAction.Display);
                }
                else
                {
                    IAction suiteAction = new GenerateTestGroupBySuiteAction(GroupPath);
                    suiteAction.Execute(TestSuite);
                    WriteVerbose(suiteAction.Display);
                }
            }
            WriteObject(TestSuite);
        }
        
        
        private ISuiteAction ProcessStaticAction(string actionName)
        {
            switch (actionName)
            {
                case "TestCaseString":
                    {
                        return new IncludeSuiteFromStringAction(TestCase, GroupPath);
                    }
                case "TestCaseFile":
                    {
                        return new IncludeSuiteAction(TestCaseFilepath, GroupPath);
                    }
                default:
                    throw new ArgumentException("Invalid ParameterSet Name");
            }
        }

        private ITemplateAction ProcessTemplate(string templateActionName)
        {
            switch (templateActionName)
            {
                case "TemplateEmbedded":
                    {
                        return new LoadEmbeddedTemplateAction(TemplateName.ToString());
                    }
                case "TemplateFile":
                    {
                        return new LoadFileTemplateAction(TemplateFilepath);
                    }
                case "TemplateString":
                    {
                        return new LoadStringTemplateAction(TemplateString);
                    }
                default:
                    throw new ArgumentException("Invalid ParameterSet Name");
            }
        }
        
        private ICaseAction ProcessCases(string caseActionName)
        {
            switch (caseActionName)
            {
                case "FromCsv":
                    {
                        return new LoadCaseFromFileAction(CsvFilepath);
                    }
                case "FromQuery":
                    {
                        return new LoadCaseFromQueryAction(Query, ConnectionString);
                    }
                case "FromQueryFile":
                    {
                        return new LoadCaseFromQueryFileAction(QueryFilepath, ConnectionString);
                    }
                case "FromTable":
                    {
                        return new LoadCaseFromDatatableAction(Datatable);
                    }
                default:
                    throw new ArgumentException("Invalid ParameterSet Name");
            }
        }
        #endregion METHODS
    }
}
