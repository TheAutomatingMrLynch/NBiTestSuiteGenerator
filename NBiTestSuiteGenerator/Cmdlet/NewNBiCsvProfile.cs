using NBi.GenbiL.Action.Setting;
using System.Collections.Generic;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Adds settings to a CSV profile in an existing NBi test suite. </para>
    /// <para type="description">Adds settings to a CSV profile in an existing NBi test suite. </para>
    /// </summary>
    /// <para type="link" uri="http://www.nbi.io/docs/config-profile-csv/">NBi documentation on NBi CSV profile</para>
    [Cmdlet(VerbsCommon.New, "NBiCsvProfile")]
    [OutputType(typeof(CsvProfile))]
    public class NewNBiCsvProfile : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">Character used for separating fields. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateLength(1,1)]
        public string FieldSeparator { get; set; }

        /// <summary>
        /// <para type="description">Character used for qualifying text. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateLength(1, 1)]
        public string TextQualifier { get; set; }

        /// <summary>
        /// <para type="description">String used for qualifying text. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string RecordSeparator { get; set; }

        /// <summary>
        /// <para type="description">Indicates whether first row of CSV file is a header. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter FirstRowHeader { get; set; }

        /// <summary>
        /// <para type="description">String used for empty cells. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string EmptyCell { get; set; }

        /// <summary>
        /// <para type="description">String used for missing cells. </para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string MissingCell { get; set; }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// Processes each record. 
        /// </summary>
        protected override void ProcessRecord()
        {
            var testSuite = new TestSuite();

            List<ISettingAction> actions = new List<ISettingAction>();


            if (MyInvocation.BoundParameters.ContainsKey("FieldSeparator"))
            {
                actions.Add(new CsvProfileFieldSeparatorAction(FieldSeparator.ToCharArray()[0]));
            }
            if (MyInvocation.BoundParameters.ContainsKey("TextQualifier"))
            {
                actions.Add(new CsvProfileTextQualifierAction(TextQualifier.ToCharArray()[0]));
            }
            if (MyInvocation.BoundParameters.ContainsKey("RecordSeparator"))
            {
                actions.Add(new CsvProfileRecordSeparatorAction(RecordSeparator));
            }
            if (MyInvocation.BoundParameters.ContainsKey("FirstRowHeader"))
            {
                actions.Add(new CsvProfileFirstRowHeaderAction(FirstRowHeader.ToBool()));
            }
            if (MyInvocation.BoundParameters.ContainsKey("EmptyCell"))
            {
                actions.Add(new CsvProfileEmptyCellAction(EmptyCell));
            }
            if (MyInvocation.BoundParameters.ContainsKey("MissingCell"))
            {
                actions.Add(new CsvProfileMissingCellAction(MissingCell));
            }

            foreach (ISettingAction action in actions)
            {
                action.Execute(testSuite);
                WriteVerbose(action.Display);
            }
            WriteObject(testSuite.Settings.CsvProfile);
        }
        #endregion METHODS
        
    }
}