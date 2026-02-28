# Logo.png -> app.ico (run in project dir, use script dir as base)
$ProjectDir = $PSScriptRoot
if ($args.Count -ge 1 -and $args[0] -match '\S') {
    $ProjectDir = $args[0].Trim().TrimEnd('\', '/', '"').Trim()
}
$png = [System.IO.Path]::Combine($ProjectDir, "Logo.png")
$ico = [System.IO.Path]::Combine($ProjectDir, "app.ico")
if (-not ([System.IO.File]::Exists($png))) { exit 0 }
try {
    Add-Type -AssemblyName System.Drawing
    $bmp = [System.Drawing.Bitmap]::FromFile($png)
    try {
        $icon = [System.Drawing.Icon]::FromHandle($bmp.GetHicon()).Clone()
        $ms = New-Object System.IO.MemoryStream
        $icon.Save($ms)
        [System.IO.File]::WriteAllBytes($ico, $ms.ToArray())
        $icon.Dispose()
    } finally {
        $bmp.Dispose()
    }
} catch {
    exit 0
}
