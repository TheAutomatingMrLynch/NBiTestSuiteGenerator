using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Adds a reference value to an existing NBi test suite. </para>
    /// <para type="description">Adds a reference value to an existing NBi test suite. </para>
    /// </summary>
    /// <para type="link" uri="http://www.nbi.io/docs/config-defaults-references/">NBi documentation on NBi references</para>
    [Cmdlet(VerbsCommon.Add, "NBiReferenceValue")]
    [OutputType(typeof(TestSuite))]
    //[CmdletBinding()]
    public class AddNBiReferenceValue : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">Test suite to add reference value to. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public TestSuite TestSuite { get; set; }

        /// <summary>
        /// <para type="description">Name of the reference. </para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Variable to set reference value for. </para>
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public string Variable { get; set; }

        /// <summary>
        /// <para type="description">Value of the reference. </para>
        /// </summary>
        [Parameter(Position = 3, Mandatory = true)]
        public string Value { get; set; }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            IAction action = new ReferenceAction(Name, Variable, Value);
            action.Execute(TestSuite);
            WriteObject(TestSuite);
            WriteVerbose(action.Display);
        }
        #endregion METHODS
    }
}