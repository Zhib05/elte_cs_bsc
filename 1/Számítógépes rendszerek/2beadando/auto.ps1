$bemenet = "adat.txt"
$elozoIdopont = $null
$megallas = 0

foreach ($sor in Get-Content $bemenet) 
{
    $oszlopok = $sor -split " "
    $ido = $oszlopok[0] + " " + $oszlopok[1]
    $aktualisIdopont = $ido -replace ":\d{3}$", ""

    if ($elozoIdopont -ne $null) 
    {
        $aktualis = [datetime]$aktualisIdopont
        $elozo = [datetime]$elozoIdopont
        $kulonbseg = ($aktualis - $elozo).TotalSeconds

        if ($kulonbseg -gt 60) 
	{
            $megallas++
        }
    }

    $elozoIdopont = $aktualisIdopont
}

Write-Output "Az autó $megallas alkalommal állt meg."
