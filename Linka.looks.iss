; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "LINKa. ������"
#define MyAppVersion "1.4.0.0"
#define MyAppPublisher "LINKa"
#define MyAppURL "http://www.linka.su"
#define MyAppExeName "linka.looks.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0C5ACB65-B21E-47C7-A88C-07C37A685873}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\LINka\LINKa.Looks
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=C:\Users\aacidov\LINKa\Releases
OutputBaseFilename=linka.looks.setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ChangesAssociations = yes
[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\aacidov\source\repos\LINKa.look-windows\LinkaWPF\bin\Release\Linka.looks.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\aacidov\source\repos\LINKa.look-windows\LinkaWPF\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\aacidov\source\repos\LINKa.look-windows\DefaultSets\*"; DestDir: "{userdocs}\LINKa"; Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
 
 [Registry]

Root: HKCR; Subkey: ".linka";                             ValueData: "{#MyAppName}";          Flags: uninsdeletevalue; ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#MyAppName}";                     ValueData: "����� �������� {#MyAppName}";  Flags: uninsdeletekey;   ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#MyAppName}\shell\open\command";  ValueData: """{app}\{#MyAppExeName}"" -p ""%1""";  ValueType: string;  ValueName: ""


Root: HKCR;  Subkey: "{#MyAppName}\DefaultIcon";        ValueData: "{app}\linka_looks_logo_mp5_icon.ico";               ValueType: string;  ValueName: "" ;

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

