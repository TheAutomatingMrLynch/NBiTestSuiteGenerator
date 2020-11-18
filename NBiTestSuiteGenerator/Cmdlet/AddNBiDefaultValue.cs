using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using System.Management.Automation;
    
namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Adds a default value to an existing NBi test suite. </para>
    /// <para type="description">Adds a default value to an existing NBi test suite. </para>
    /// </summary>
    /// <para type="link" uri="http://www.nbi.io/docs/config-defaults-references/">NBi documentation on NBi defaults</para>

    [Cmdlet(VerbsCommon.Add, "NBiDefaultValue")]
    [OutputType(typeof(TestSuite))]
    //[CmdletBinding()]
    public class AddNBiDefaultValue : PSCmdlet
    {
        #region FIELDS
        private DefaultType _scope = DefaultType.Everywhere;
        #endregion FIELDS

        #region PROPERTIES
        /// <summary>
        /// <para type="description">Test suite to add default value to. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public TestSuite TestSuite { get; set; }

        /// <summary>
        /// <para type="description">Variable to set default value for. </para>
        /// </summary>
        [Parameter(Position = 1, Mandatory = true)]
        public string Variable { get; set; }

        /// <summary>
        /// <para type="description">The default value of the variable. </para>
        /// </summary>
        [Parameter(Position = 2, Mandatory = true)]
        public string Value { get; set; }

        /// <summary>
        /// <para type="description">Scope of the default. </para>
        /// </summary>
        [Parameter(Position = 3, Mandatory = false)]
        public DefaultType Scope { get { return _scope; } set { _scope = value; } }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            IAction action = new DefaultAction(Scope, Variable, Value);
            action.Execute(TestSuite);
            WriteObject(TestSuite);
            WriteVerbose(action.Display);
        }
        #endregion METHODS
    }
}