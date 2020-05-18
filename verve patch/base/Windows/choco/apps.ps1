#################################################
# Thanks to vintem 
#################################################

Write-Host "Installing applications from Chocolatey"
cinst git -y
cinst ruby -y
cinst nodejs -y
cinst GoogleEarth -y
cinst webpi -y
cinst poshgit -y
cinst sublimetext3 -y
cinst ConEmu -y
cinst python -y
choco install easy.install -y
cinst putty -y
cinst GoogleChrome -y
choco install firefox -y
cinst fiddler4 -y
cinst filezilla -y
cinst dropbox -y
cinst winmerge -y
cinst kdiff3 -y
cinst winrar -Version 4.20.0 -y
cinst mongodb -y
cinst NugetPackageExplorer -y
cinst Evernote -y
choco install brackets -y
choco install lastpass -y
Write-Host

######################################################
# Set environment variables
######################################################
Write-Host "Setting home variable"
[Environment]::SetEnvironmentVariable("HOME", $HOME, "User")
[Environment]::SetEnvironmentVariable("CHROME_BIN", "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "User")
[Environment]::SetEnvironmentVariable("PHANTOMJS_BIN", "C:\tools\PhanomJS\phantomjs.exe", "User")
Write-Host

######################################################
# Download custom .bashrc file
######################################################
Write-Host "Creating .bashrc file for use with Git Bash"
$filePath = $HOME + "\.bashrc"
New-Item $filePath -type file -value ((new-object net.webclient).DownloadString('http://bit.ly/winbashrc'))
Write-Host

######################################################
# Install Windows installer through WebPI
######################################################
Write-Host "Installing apps from WebPI"
cinst WindowsInstaller31 -source webpi
cinst WindowsInstaller45 -source webpi
Write-Host

######################################################
# Install SQL Express 2012
######################################################
Write-Host
do {
    $createSiteData = Read-Host "Do you want to install SQLExpress? (Y/N)"
} while ($createSiteData -ne "Y" -and $createSiteData -ne "N")
if ($createSiteData -eq "Y") {
    cinst SqlServer2012Express
}
Write-Host

######################################################
# Add Git to the path
######################################################
Write-Host "Adding Git\bin to the path"
Add-Path "C:\Program Files (x86)\Git\bin"
Write-Host

######################################################
# Configure Git globals
######################################################
Write-Host "Configuring Git globals"
$userName = Read-Host 'Enter your name for git configuration'
$userEmail = Read-Host 'Enter your email for git configuration'

& 'C:\Program Files (x86)\Git\bin\git' config --global user.email $userEmail
& 'C:\Program Files (x86)\Git\bin\git' config --global user.name $userName

$gitConfig = $home + "\.gitconfig"
Add-Content $gitConfig ((new-object net.webclient).DownloadString('http://bit.ly/mygitconfig'))

$gitexcludes = $home + "\.gitexcludes"
Add-Content $gitexcludes ((new-object net.webclient).DownloadString('http://bit.ly/gitexcludes'))
Write-Host

$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine")

######################################################
# Update RubyGems and install some gems
######################################################
Write-Host "Update RubyGems"
choco install ruby2.devkit -y
gem update --system
gem install bundler
Write-Host

######################################################
# Install npm packages
######################################################
Write-Host "Install NPM packages"
npm install -g yo gulp karma bower jshint nodemon
Write-Host

######################################################
# Generate public/private rsa key pair
######################################################
Write-Host "Generating public/private rsa key pair"
Set-Location $home
$dirssh = "$home\.ssh"
mkdir $dirssh
$filersa = $dirssh + "\id_rsa"
ssh-keygen -t rsa -f $filersa -q -C $userEmail
Write-Host

######################################################
# Add MongoDB to the path
######################################################
Write-Host "Adding MongoDB to the path"
Add-Path "C:\MongoDB\bin"
Write-Host

######################################################
# Download custom PowerShell profile file
######################################################
Write-Host "Creating custom $profile for Powershell"
if (!(test-path $profile)) {
    New-Item -path $profile -type file -force
}
Add-Content $profile ((new-object net.webclient).DownloadString('http://bit.ly/profileps'))
Write-Host