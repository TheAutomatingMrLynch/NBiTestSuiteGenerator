using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Action.Setting.CsvProfile;
using System.Collections.Generic;
using System.Management.Automation;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Creates a new CSV profile. </para>
    /// <para type="description">Creates a new CSV profile. </para>
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

            var actions = new List<ICsvProfileAction>();


            if (MyInvocation.BoundParameters.ContainsKey("FieldSeparator"))
            {
                actions.Add(new FieldSeparatorAction(FieldSeparator.ToCharArray()[0]));
            }
            if (MyInvocation.BoundParameters.ContainsKey("TextQualifier"))
            {
                actions.Add(new TextQualifierAction(TextQualifier.ToCharArray()[0]));
            }
            if (MyInvocation.BoundParameters.ContainsKey("RecordSeparator"))
            {
                actions.Add(new RecordSeparatorAction(RecordSeparator));
            }
            if (MyInvocation.BoundParameters.ContainsKey("FirstRowHeader"))
            {
                actions.Add(new FirstRowHeaderAction(FirstRowHeader.ToBool()));
            }
            if (MyInvocation.BoundParameters.ContainsKey("EmptyCell"))
            {
                actions.Add(new EmptyCellAction(EmptyCell));
            }
            if (MyInvocation.BoundParameters.ContainsKey("MissingCell"))
            {
                actions.Add(new MissingCellAction(MissingCell));
            }

            foreach (var action in actions)
            {
                action.Execute(testSuite);
                WriteVerbose(action.Display);
            }
            WriteObject(testSuite.Settings.CsvProfile);
        }
        #endregion METHODS
        
    }
}