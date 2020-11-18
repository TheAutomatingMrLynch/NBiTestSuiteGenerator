using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Suite;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Imports test cases from an existing NBi test suite file (.nbits). </para>
    /// <para type="description">Imports test cases from an existing NBi test suite file (.nbits). </para>
    /// </summary>
    [Cmdlet(VerbsData.Import, "NBiTestSuite")]
    [OutputType(typeof(TestSuite))]
    //[CmdletBinding()]
    public class ImportNBiTestSuite : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">Test suite to add test cases to. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public TestSuite TestSuite { get; set; }

        /// <summary>
        /// <para type="description">File path to an existing NBi test suite file (.nbits). </para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string SourceFilepath { get; set; }

        /// <summary>
        /// <para type="description">Optional group path where the imported test cases will be placed. </para>
        /// </summary>
        [Parameter(Position = 2, Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string GroupPath { get; set; }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            IAction action = new AddRangeSuiteAction(SourceFilepath, GroupPath);
            action.Execute(TestSuite);
            WriteObject(TestSuite);
            WriteVerbose(action.Display);
        }
        #endregion METHODS
    }
}