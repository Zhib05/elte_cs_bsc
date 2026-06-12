#!/bin/bash

echo -e "x\tcos(x)\tsin(x)"

for i in $(seq 1 10); do
    x=$(echo "scale=1; $i/10" | bc -l)
    cos=$(echo "scale=5; c($x)" | bc -l)
    sin=$(echo "scale=5; s($x)" | bc -l)
    echo -e "$x\t$cos\t$sin"
done
#pi=$(echo "scale=10; 4*a(1)" | bc -l)
