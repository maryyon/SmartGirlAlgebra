Add-Type -AssemblyName System.Drawing

Get-ChildItem -Path "SmartGirlAlgebra/wwwroot/images" -Filter "*.png" | ForEach-Object {
    $img = New-Object System.Drawing.Bitmap($_.FullName)
    $hasTransparentPixels = $false
    
    # Sample pixels to check for transparency (checking every 10th pixel for speed)
    for ($x = 0; $x -lt $img.Width -and !$hasTransparentPixels; $x += 10) {
        for ($y = 0; $y -lt $img.Height -and !$hasTransparentPixels; $y += 10) {
            $pixel = $img.GetPixel($x, $y)
            if ($pixel.A -lt 255) {
                $hasTransparentPixels = $true
            }
        }
    }
    
    $img.Dispose()
    
    [PSCustomObject]@{
        Name = $_.Name
        PixelFormat = "Format32bppArgb"
        HasTransparentPixels = $hasTransparentPixels
    }
}

