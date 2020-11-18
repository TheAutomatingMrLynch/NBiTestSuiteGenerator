using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBiTestSuiteGenerator
{
    /// <summary>
    /// Contains names of embedded NBi test case templates. 
    /// </summary>
    public class EmbeddedTemplate
    {
        /// <summary>
        /// Valid names for embedded NBi test case templates. 
        /// </summary>
        public enum Template 
        {
            /// <summary>
            /// Assert that all the dimensions visible through the perspective '$perspective$' are in the following list of $length(dimension)$ expected dimensions: $dimension; separator=", "$.
            /// </summary>
            ContainedInDimensions,
            /// <summary>
            /// Assert that all the hierarchies in dimension '$dimension$' visible through the perspective '$perspective$' are in the following list of $length(hierarchy)$ expected hierarchies: $hierarchy; separator=", "$.
            /// </summary>
            ContainedInHierarchies,
            /// <summary>
            /// Assert that all the levels of hierarchy '$hierarchy$' in dimension '$dimension$' visible through the perspective '$perspective$' are in the following list of $length(level)$ expected levels: $level; separator=", "$.
            /// </summary>
            ContainedInLevels,
            /// <summary>
            /// Assert that all the measure-groups visible through the perspective '$perspective$' are in the following list of $length(measuregroup)$ expected measure-groups: $measuregroup; separator=", "$.
            /// </summary>
            ContainedInMeasureGroup,
            /// <summary>
            /// Assert that all the measures in the  measure-group '$measuregroup$' visible through the perspective '$perspective$' are in the following list of $length(measure)$ expected measures: $measure; separator=", "$.
            /// </summary>
            ContainedInMeasures,
            /// <summary>
            /// Assert that all the perspectives are in the following list of $length(perspective)$ expected perspectives: $perspective; separator=", "$.
            /// </summary>
            ContainedInPerspectives,
            /// <summary>
            /// Assert that the dimension named '$dimension$' exists through the perspective '$perspective$' and is visible for end-users
            /// </summary>
            ExistsDimension,
            /// <summary>
            /// Assert that the hierarchy named '$hierarchy$' exists in dimension '$dimension$' through the perspective '$perspective$' and is visible for end-users
            /// </summary>
            ExistsHierarchy,
            /// <summary>
            /// Assert that the level named '$level$' exists in hierarchy '$hierarchy$', in dimension '$dimension$' through the perspective '$perspective$' and is visible for end-users
            /// </summary>
            ExistsLevel,
            /// <summary>
            /// Assert that the measure named '$measure$' exists under the folder '$displayfolder$' in measure-group '$measuregroup$' through the perspective '$perspective$' and is visible for end-users
            /// </summary>
            ExistsMeasure,
            /// <summary>
            /// Assert that the measure-group named '$measuregroup$' exists through the perspective '$perspective$' and is visible for end-users
            /// </summary>
            ExistsMeasureGroup,
            /// <summary>
            /// Assert that the perspective '$perspective$' exists and is visible for end-users
            /// </summary>
            ExistsPerspective,
            /// <summary>
            /// Assert that the dimension named '$dimension$' is linked to a measure-group named '$measuregroup$' in perspective '$perspective$'
            /// </summary>
            LinkedToDimension,
            /// <summary>
            /// Assert that the measure-group named '$measuregroup$' is linked to a dimension named '$dimension$' in perspective '$perspective$'
            /// </summary>
            LinkedToMeasureGroup,
            /// <summary>
            /// Check if the same query executed on two instances of the cube have the same result. Specifically, for measure '$measure$' by dimension/hierarchy '$dimension$/$hierarchy$' through perspective '$perspective$'
            /// </summary>
            QueryEqualToItself
        }
    }
}
