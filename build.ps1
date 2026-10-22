$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $compiler)) {
    throw 'The .NET Framework x64 compiler was not found. This project requires Windows x64 and .NET Framework 4.8.'
}
$sourcePath = Join-Path $PSScriptRoot 'JblVolume.cs'
$outputPath = Join-Path $PSScriptRoot 'JBLVolume.exe'
& $compiler /nologo /target:winexe /platform:x64 /optimize+ "/out:$outputPath" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll $sourcePath
if ($LASTEXITCODE -ne 0) { throw 'Compilation failed.' }
Get-FileHash -LiteralPath $outputPath -Algorithm SHA256
