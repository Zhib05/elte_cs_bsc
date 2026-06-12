#!/bin/bash
num=$1

if ! test $1 -ge 1 -a $1 -le 20; then
    echo "1 és 20 között kell lennie."
    exit 1
fi

 for((i=1; i<=num; i++)); do
    for((j=1; j<=i; j++)); do
	echo -n "*"
    done
    echo
 done

 for((i=num-1; i>=1; i--)); do
    for((j=1; j<=i; j++));do
	echo -n "*"
    done
    echo
 done
