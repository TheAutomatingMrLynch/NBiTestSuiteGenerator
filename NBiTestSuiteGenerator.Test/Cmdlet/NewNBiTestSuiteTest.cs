using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiTestSuite")]
    public class NewNBiTestSuiteTest
    {
        #region FIELDS
        private Runspace _runSpace;
        private string _assemblyPath;
        #endregion FIELDS

        #region METHODS

        #region Setup and teardown
        [OneTimeSetUp]
        public void Setup()
        {
            _runSpace = RunspaceFactory.CreateRunspace();
            _runSpace.Open();

            _assemblyPath = typeof(NewNBiTestSuite).Assembly.Location;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _runSpace.Close();
        }
        #endregion Setup and teardown

        #region Tests
        [Test]
        public void NewNBiTestSuite_ReturnsTestSuite()
        {
            // Arrange
            Type expected = typeof(GenerationState);

            string script = $@"
            Import-Module { _assemblyPath }
            New-NBiTestSuite";

            Pipeline pipeline = _runSpace.CreatePipeline(script);
            
            // Act
            var result = pipeline.Invoke();
            var actual = result.FirstOrDefault().BaseObject;
            
            // Assert
            Assert.IsInstanceOf(expected, actual);
        }
        #endregion Tests

        #endregion METHODS
    }
}