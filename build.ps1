$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $compiler)) {
    throw 'No se encontro el compilador .NET Framework x64. Este proyecto requiere Windows x64 y .NET Framework 4.8.'
}
$sourcePath = Join-Path $PSScriptRoot 'JblVolume.cs'
$outputPath = Join-Path $PSScriptRoot 'JBLVolume.exe'
& $compiler /nologo /target:winexe /platform:x64 /optimize+ "/out:$outputPath" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll $sourcePath
if ($LASTEXITCODE -ne 0) { throw 'La compilacion fallo.' }
Get-FileHash -LiteralPath $outputPath -Algorithm SHA256
