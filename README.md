# NBiTestSuiteGenerator
NBiTestSuiteGenerator is a PowerShell module for auto generating [NBi](https://www.NBi.io) test suites based on metadata. This project relies heavily on the work of Cédric L. Charlier ([twitter](https://www.twitter.com/seddryck) | [blog](https://seddryck.wordpress.com) | [gihub](https://github.com/Seddryck)) who has developed the NBi test framework. He is not a maintainer on this project but he has been kind enough to add a few changes in NBi to make this project a possibility. 

# Typical use-cases 
If you want to unit test your database/OLAP cubes/Semantic models etc. you can do so even without this module. NBi even has its' own language for generating test suites (GenbiL). Some of the advantages of this module over GenbiL are:
- Generate test suites using the familiar syntax of PowerShell. Now you only have to learn the syntax of an NBi test case
- Any transformation of raw metadata can be done on the fly. This is very handy in case of multiple sources of metadata that has to be merged first
- All input like metadata or templates can be kept in memory so no need to access local files. You can of course use local files if you want - your choice 

# Included cmdlets
## Test suite
- **New-NBiTestSuite**: Generates an empty NBi test suite
- **Import-NBiTestSuite**: Imports test cases from an existing NBi test suite file (.nbits)
- **Save-NBiTestSuite**: Saves an NBi test suite as a file

## Test case
- **Add-NBiTestcase**: Adds a set of test cases to an NBi test suite

## Settings
- **Add-NBiReferenceValue**: Adds a reference value to an existing NBi test suite
- **Add-NBiDefaultValue**: Adds a default value to an existing NBi test suite
- **Add-NBiCsvProfile**: Adds settings to a CSV profile in an existing NBi test suite. 

# Reporting bugs and change requests
If you experience any bugs or think a particular feature should be included in this module please open an issue with a good description and if possible steps to recreate in case of a bug. While this module is used where I work during the day this project is mainly maintained in my spare time. So please allow some response time before spamming me. :-) 
