using NBi.GenbiL.Action.Setting;
using NBi.Xml;
using NBi.Xml.Settings;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Xml.Serialization;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// <para type="synopsis">Saves a CSV profile to a file. </para>
    /// <para type="description">Saves a CSV profile to a file. </para>
    /// </summary>
    /// <para type="link" uri="http://www.nbi.io/docs/config-profile-csv/">NBi documentation on NBi CSV profile</para>
    [Cmdlet(VerbsData.Save, "NBiCsvProfile")]
    [OutputType(typeof(CsvProfile))]
    public class SaveNBiCsvProfile : PSCmdlet
    {
        #region PROPERTIES
        /// <summary>
        /// <para type="description">CSV profile to save. </para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public CsvProfileXml CsvProfile { get; set; }

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
            var serializer = new XmlSerializer(typeof(CsvProfileXml));

            using (var writer = new StreamWriter(Filepath, false, Encoding.UTF8))
            {
                serializer.Serialize(writer, CsvProfile);
            }
        }
        #endregion METHODS

    }
}