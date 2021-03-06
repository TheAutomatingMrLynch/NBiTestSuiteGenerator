﻿using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiTestSuite")]
    public class SaveNBiTestSuiteTest : CmdletTestBase
    {
        #region METHODS

        #region Tests
        [Test]
        public void SaveNBiTestSuite_SavesFile()
        {
            // Arrange
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveNBiTestSuite_SavesFile.nbits");
            if (File.Exists(filepath)) { File.Delete(filepath); }

            string script = $@"
            Import-Module { AssemblyPath }
            $testSuite = New-NBiTestSuite
            Save-NBiTestSuite -TestSuite $testSuite -Filepath { filepath }";

            Pipeline pipeline = Runspace.CreatePipeline(script);

            try
            {
                // Act
                pipeline.Invoke();

                // Assert
                FileAssert.Exists(filepath);
            }
            // Cleanup
            finally
            {
                if (File.Exists(filepath)) { File.Delete(filepath); }
            }
        }

        [Test]
        [TestCase("//d:testSuite", TestName = "SaveNBiTestSuite_CorrectFileStructure_TestSuiteNode")]
        [TestCase("//d:settings", TestName = "SaveNBiTestSuite_CorrectFileStructure_SettingsNode")]
        public void SaveNBiTestSuite_CorrectFileStructure(string xpath)
        {
            // Arrange
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveNBiTestSuite_CorrectFileStructure.nbits");
            if (File.Exists(filepath)) { File.Delete(filepath); }

            string script = $@"
            Import-Module { AssemblyPath }
            $testSuite = New-NBiTestSuite
            Save-NBiTestSuite -TestSuite $testSuite -Filepath { filepath }";

            Pipeline pipeline = Runspace.CreatePipeline(script);

            try
            {
                // Act
                pipeline.Invoke();

                // Assert
                IEnumerable actual;
                using (FileStream stream = File.OpenRead(filepath))
                {
                    XDocument xdoc = XDocument.Load(stream);
                    XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
                    manager.AddNamespace("d", "http://NBi/TestSuite");
                    actual = (IEnumerable)xdoc.Root.XPathEvaluate(xpath, manager);
                }

                CollectionAssert.IsNotEmpty(actual);
            }
            // Cleanup
            finally
            {
                if (File.Exists(filepath)) { File.Delete(filepath); }
            }
        }
        #endregion Tests

        #endregion METHODS
    }
}