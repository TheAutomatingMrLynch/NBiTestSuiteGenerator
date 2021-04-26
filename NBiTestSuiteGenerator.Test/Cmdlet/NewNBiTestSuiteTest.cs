using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace NBiTestSuiteGenerator.Test
{
    [TestFixture(Category = "NBiTestSuite")]
    public class NewNBiTestSuiteTest : CmdletTestBase
    {
        #region METHODS

        #region Tests
        [Test]
        public void NewNBiTestSuite_ReturnsTestSuite()
        {
            // Arrange
            Type expected = typeof(GenerationState);

            string script = $@"
            Import-Module { AssemblyPath }
            New-NBiTestSuite";

            Pipeline pipeline = Runspace.CreatePipeline(script);
            
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