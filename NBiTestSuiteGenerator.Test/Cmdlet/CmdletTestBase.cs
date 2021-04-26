using NUnit.Framework;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    public abstract class CmdletTestBase
    {
        #region PROPERTIES
        public Runspace Runspace { get; private set; }
        public string AssemblyPath { get; private set; }
        #endregion PROPERTIES

        #region METHODS

        #region Setup and teardown
        [OneTimeSetUp]
        public void Setup()
        {
            Runspace = RunspaceFactory.CreateRunspace();
            Runspace.Open();

            AssemblyPath = typeof(TestSuite).Assembly.Location;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Runspace.Close();
        }
        #endregion Setup and teardown

        #endregion METHODS
    }
}