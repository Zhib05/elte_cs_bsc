#!/bin/bash

input="adat.txt"
megallas=0
ido=""

while read -r ido1 _;
 do
   if [ -n "$ido" ]; then
	aktualis=$(date -d "$ido1" +%s)
	elozo=$(date -d "$ido" +%s)

	if [ -n "$aktualis" -a -n "$elozo" ]; then
		kulonbseg=$((aktualis - elozo))

		if ((kulonbseg > 60)); then
	    	    ((megallas++))
		fi
	fi
   fi

   ido="$ido1"
 done < "$input"

echo "Az auto $megallas allt meg"
