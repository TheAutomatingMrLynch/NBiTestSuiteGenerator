using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Generates an empty NBi test suite. </para>
    /// <para type="description">Generates an empty NBi test suite. </para>
    /// <para type="link" uri="http://www.nbi.io/docs/installation-test-suite/">NBi documentation on the .nbits format</para>
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NBiTestSuite")]
    [OutputType(typeof(TestSuite))]
    //[CmdletBinding()]
    public class NewNBiTestSuite : PSCmdlet
    {
        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject(new TestSuite());
        }
        #endregion METHODS
    }
}