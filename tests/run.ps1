$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$source = Join-Path $PSScriptRoot '..\JblVolume.cs'
$testSource = Join-Path $PSScriptRoot 'VolumePolicyTests.cs'
$testOutput = Join-Path $env:TEMP ('JBLVolume-tests-' + [Guid]::NewGuid().ToString('N') + '.exe')
try {
    & $compiler /nologo /target:exe /platform:x64 /main:JblVolume.VolumePolicyTests "/out:$testOutput" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll $source $testSource
    if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
    & $testOutput
    if ($LASTEXITCODE -ne 0) { throw 'Volume policy regression tests failed.' }
} finally {
    if (Test-Path -LiteralPath $testOutput) { Remove-Item -LiteralPath $testOutput }
}
