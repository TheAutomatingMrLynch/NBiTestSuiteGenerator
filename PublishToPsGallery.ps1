$ErrorActionPreference = 'Stop'

Import-Module -Name PackageManagement -Force -MinimumVersion '1.4'

$buildFolder  = "$PSScriptRoot\NBiTestSuiteGenerator\bin\Release"
$moduleFolder = "$PSScriptRoot\NBiTestSuiteGenerator\ModuleStage\NBiTestSuiteGenerator"
$year = Get-Date -Format "yyyy"
$author = 'automated.dk'

#Write-Host "Generate NBi.PowerShell.dll-Help.xml (for Get-Help on the module)"
#& "$($PSScriptRoot)\packages\XmlDoc2CmdletDoc.0.3.0\tools\XmlDoc2CmdletDoc.exe $buildFolder"

# Create manifest file
$parameters = @{
    Path                    = "$buildFolder\NBiTestSuiteGenerator.psd1"
    RootModule              = 'NBiTestSuiteGenerator.dll'
    ModuleVersion           = '0.2.3'
    Guid                    = 'bad43db4-c442-456b-97f9-98b3ee4d6450'
    Author                  = $author
    CompanyName             = $author
    Copyright               = "(c) $year $author. All rights reserved."
    Description             = 'NBiTestSuiteGenerator is a PowerShell module for auto generating NBi test suites based on metadata. '
    CmdletsToExport         = @(
        'New-NBiTestSuite', 'Save-NBiTestSuite', 'Import-NBiTestSuite', 
        'Add-NBiTestcase', 
        'Add-NBiDefaultValue', 
        'Add-NBiReferenceValue', 
        'New-NBiCsvProfile', 'Add-NBiCsvProfile', 'Get-NBiCsvProfile', 'Save-NBiCsvProfile'
      )
    
    ReleaseNotes            = 'Fixes incorrect serializing of CSV profiles. '
    Tags                    = @('NBi', 'TestSuite', 'Generator')
    LicenseUri              = 'https://raw.githubusercontent.com/TheAutomatingMrLynch/NBiTestSuiteGenerator/main/LICENSE'
    IconUri                 = 'https://raw.githubusercontent.com/TheAutomatingMrLynch/NBiTestSuiteGenerator/main/images/logo.png'
    ProjectUri              = 'https://github.com/TheAutomatingMrLynch/NBiTestSuiteGenerator'
}
New-ModuleManifest @parameters 

# Clean module folder and copy builded files
$moduleFolder | where { Test-Path -Path $_} | Remove-Item -Recurse -Force 
Write-Host "Copying files from '$buildFolder' to '$moduleFolder'. "
Copy-Item -Path "$buildFolder" -Destination "$moduleFolder" -Recurse -Force

# Publish module
$nuGetApiKey = Read-Host -Prompt 'Enter API key' #-AsSecureString
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Publish-Module -Path $moduleFolder -NuGetApiKey $nuGetApiKey -Repository PSGallery -Verbose 