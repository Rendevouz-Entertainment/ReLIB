dotnet build

Remove-Item "..\ModBuild\ReLIB" -Recurse

New-Item "..\ModBuild\ReLIB" -ItemType "directory"
New-Item "..\ModBuild\ReLIB\BepInEx" -ItemType "directory"

New-Item "..\ModBuild\ReLIB\BepInEx\patchers" -ItemType "directory"
New-Item "..\ModBuild\ReLIB\BepInEx\patchers\ReLIBPatcher" -ItemType "directory"

New-Item "..\ModBuild\ReLIB\BepInEx\plugins" -ItemType "directory"
New-Item "..\ModBuild\ReLIB\BepInEx\plugins\ReLIB" -ItemType "directory"



Copy-Item "..\ReLIBPatcher\bin\Debug\netstandard2.1\ReLIB Patcher.dll" "..\ModBuild\ReLIB\BepInEx\patchers\ReLIBPatcher"
Copy-Item "..\ReLIBPatcher\bin\Debug\netstandard2.1\ReLIB Patcher.pdb" "..\ModBuild\ReLIB\BepInEx\patchers\ReLIBPatcher"

Copy-Item ".\bin\Debug\netstandard2.1\ReLIB.dll" "..\ModBuild\ReLIB\BepInEx\plugins\ReLIB"
Copy-Item ".\bin\Debug\netstandard2.1\ReLIB.pdb" "..\ModBuild\ReLIB\BepInEx\plugins\ReLIB"

Copy-Item "..\ModBuild\ModData\ReLIB\*" "..\ModBuild\ReLIB\BepInEx\plugins\ReLIB" -Force -Recurse


Copy-Item "..\ModBuild\ReLIB\BepInEx\*" "..\Game\BepInEx" -Recurse -Force

Start-Process "..\Game\KSP2_x64.exe"