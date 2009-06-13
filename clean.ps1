foreach ($file in (Get-ChildItem . -Recurse -Include Bin, Obj, *.cachefile, *.user))
{
    "Removing " + $file.name
    Remove-Item $file -Recurse
}