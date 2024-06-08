function GenerateResourceClass {
    param (
        [string]$resourceFolderPath,
        [string]$resourceFileName,
        [string]$namespace,
        [string]$outputFile
    )

    $generatedClassName = "${resourceFileName}Generated"
    $resourceFiles = Get-ChildItem -Path $resourceFolderPath -Filter "$resourceFileName.*.resx"

    $contents = @"
namespace $namespace
{
    public static class $generatedClassName
    {

"@

    $uniqueKeys = @{}
    foreach ($file in $resourceFiles) {
        [xml]$resxContent = Get-Content $file.FullName
        foreach ($data in $resxContent.root.data) {
            $name = $data.name

            if (-not $uniqueKeys.ContainsKey($name)) {
                $uniqueKeys[$name] = $true
                $contents += "        public const string $name = `"$name`";" + "`r`n"
            }
        }
    }

    $contents += @"
    }
}
"@

    Set-Content -Path $outputFile -Value $contents
}

function GenerateResourceClasses {
    param (
        [string]$resourceFolderPath,
        [string]$namespace
    )

    $resourceGroups = Get-ChildItem -Path $resourceFolderPath -Filter "*.resx" | 
                      Group-Object { $_.BaseName -replace '\..*$', '' }

    foreach ($group in $resourceGroups) {
        $resourceFileName = $group.Name
        $outputFile = "$resourceFileName.generated.cs"
        GenerateResourceClass -resourceFolderPath $resourceFolderPath -resourceFileName $resourceFileName -namespace $namespace -outputFile $outputFile
    }
}

$namespace = "AxiteHR.GlobalizationResources"
$resourceFolderPath = ".\Resources"

GenerateResourceClasses -resourceFolderPath $resourceFolderPath -namespace $namespace