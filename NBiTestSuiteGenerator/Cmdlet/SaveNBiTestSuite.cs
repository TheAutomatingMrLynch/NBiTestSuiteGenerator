using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Suite;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Saves an NBi test suite as a file. </para>
    /// <para type="description">Saves an NBi test suite as a file. </para>
    /// </summary>
    [Cmdlet(VerbsData.Save, "NBiTestSuite")]
    [OutputType(typeof(string))]
    //[CmdletBinding()]
    public class SaveNBiTestSuite : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">Test suite to be saved. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public TestSuite TestSuite { get; set; }

        /// <summary>
        /// <para type="description">File path where the file will be saved. </para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Filepath { get; set; }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            IAction action = new SaveSuiteAction(Filepath);
            action.Execute(TestSuite);
            WriteObject(Filepath);
            WriteVerbose(action.Display);
        }
        #endregion METHODS
    }
}