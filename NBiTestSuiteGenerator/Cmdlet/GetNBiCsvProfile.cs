using NBi.GenbiL.Action.Setting;
using System.Collections.Generic;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Gets a CSV profile from an existing NBi test suite. </para>
    /// <para type="description">Gets a CSV profile from an existing NBi test suite. </para>
    /// </summary>
    /// <para type="link" uri="http://www.nbi.io/docs/config-profile-csv/">NBi documentation on NBi CSV profile</para>
    [Cmdlet(VerbsCommon.Get, "NBiCsvProfile")]
    [OutputType(typeof(CsvProfile))]
    public class GetNBiCsvProfile : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">Test suite to get CSV profile from. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public TestSuite TestSuite { get; set; }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject(TestSuite.Settings.CsvProfile);
        }
        #endregion METHODS
        
    }
}