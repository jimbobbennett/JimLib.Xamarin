Param
(
    [Parameter(Mandatory=$True)][string]$version,
    [bool]$push = $false,
    [string]$copyTo
) 

& 'C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe' .\JimLib.Xamarin.sln /property:Configuration=Release /Target:Rebuild
& 'nuget.exe' pack .\JimBobBennett.JimLib.Xamarin.nuspec -Properties Configuration=Release -Version $version -Symbols

if ($copyTo)
{
    cp .\JimBobBennett.JimLib.Xamarin.$version.nupkg $copyTo
    cp .\JimBobBennett.JimLib.Xamarin.$version.symbols.nupkg $copyTo
}


#if ($push)
#{
#    nuget.exe push .\JimBobBennett.JimLib.Xamarin.$version.nupkg
#}