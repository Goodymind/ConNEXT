function Get-FileName
{
<#
.SYNOPSIS
   Show an Open File Dialog and return the file selected by the user

.DESCRIPTION
   Show an Open File Dialog and return the file selected by the user

.PARAMETER WindowTitle
   Message Box title
   Mandatory - [String]

.PARAMETER InitialDirectory
   Initial Directory for browsing
   Mandatory - [string]

.PARAMETER Filter
   Filter to apply
   Optional - [string]

.PARAMETER AllowMultiSelect
   Allow multi file selection
   Optional - switch

 .EXAMPLE
   Get-FileName
    cmdlet Get-FileName at position 1 of the command pipeline
    Provide values for the following parameters:
    WindowTitle: My Dialog Box
    InitialDirectory: c:\temp
    C:\Temp\42258.txt

    No passthru paramater then function requires the mandatory parameters (WindowsTitle and InitialDirectory)

.EXAMPLE
   Get-FileName -WindowTitle MyDialogBox -InitialDirectory c:\temp
   C:\Temp\41553.txt

   Choose only one file. All files extensions are allowed

.EXAMPLE
   Get-FileName -WindowTitle MyDialogBox -InitialDirectory c:\temp -AllowMultiSelect
   C:\Temp\8544.txt
   C:\Temp\42258.txt

   Choose multiple files. All files are allowed

.EXAMPLE
   Get-FileName -WindowTitle MyDialogBox -InitialDirectory c:\temp -AllowMultiSelect -Filter "text file (*.txt) | *.txt"
   C:\Temp\AES_PASSWORD_FILE.txt

   Choose multiple files but only one specific extension (here : .txt) is allowed

.EXAMPLE
   Get-FileName -WindowTitle MyDialogBox -InitialDirectory c:\temp -AllowMultiSelect -Filter "Text files (*.txt)|*.txt| csv files (*.csv)|*.csv | log files (*.log) | *.log"
   C:\Temp\logrobo.log
   C:\Temp\mylogfile.log

   Choose multiple file with the same extension

.EXAMPLE
   Get-FileName -WindowTitle MyDialogBox -InitialDirectory c:\temp -AllowMultiSelect -Filter "selected extensions (*.txt, *.log) | *.txt;*.log"
   C:\Temp\IPAddresses.txt
   C:\Temp\log.log

   Choose multiple file with different extensions
   Nota :It's important to have no white space in the extension name if you want to show them

.EXAMPLE
 Get-Help Get-FileName -Full

.INPUTS
   System.String
   System.Management.Automation.SwitchParameter

.OUTPUTS
   System.String

.NOTESs
  Version         : 1.0
  Author          : O. FERRIERE
  Creation Date   : 11/09/2019
  Purpose/Change  : Initial development

  Based on different pages :
   mainly based on https://blog.danskingdom.com/powershell-multi-line-input-box-dialog-open-file-dialog-folder-browser-dialog-input-box-and-message-box/
   https://code.adonline.id.au/folder-file-browser-dialogues-powershell/
   https://thomasrayner.ca/open-file-dialog-box-in-powershell/
#>
    [CmdletBinding()]
    [OutputType([string])]
    Param
    (
        # WindowsTitle help description
        [Parameter(
            Mandatory = $true,
            ValueFromPipelineByPropertyName = $true,
            HelpMessage = "Message Box Title",
            Position = 0)]
        [String]$WindowTitle,

        # InitialDirectory help description
        [Parameter(
            Mandatory = $true,
            ValueFromPipelineByPropertyName = $true,
            HelpMessage = "Initial Directory for browsing",
            Position = 1)]
        [String]$InitialDirectory,

        # Filter help description
        [Parameter(
            Mandatory = $false,
            ValueFromPipelineByPropertyName = $true,
            HelpMessage = "Filter to apply",
            Position = 2)]
        [String]$Filter = "All files (*.*)|*.*",

        # AllowMultiSelect help description
        [Parameter(
            Mandatory = $false,
            ValueFromPipelineByPropertyName = $true,
            HelpMessage = "Allow multi files selection",
            Position = 3)]
        [Switch]$AllowMultiSelect
    )

    # Load Assembly
    Add-Type -AssemblyName System.Windows.Forms

    # Open Class
    $OpenFileDialog = New-Object System.Windows.Forms.OpenFileDialog

    # Define Title
    $OpenFileDialog.Title = $WindowTitle

    # Define Initial Directory
    if (-Not [String]::IsNullOrWhiteSpace($InitialDirectory))
    {
        $OpenFileDialog.InitialDirectory = $InitialDirectory
    }

    # Define Filter
    $OpenFileDialog.Filter = $Filter

    # Check If Multi-select if used
    if ($AllowMultiSelect)
    {
        $OpenFileDialog.MultiSelect = $true
    }
    $OpenFileDialog.ShowHelp = $true    # Without this line the ShowDialog() function may hang depending on system configuration and running from console vs. ISE.
    $OpenFileDialog.ShowDialog() | Out-Null
    if ($AllowMultiSelect)
    {
        return $OpenFileDialog.Filenames
    }
    else
    {
        return $OpenFileDialog.Filename
    }
}