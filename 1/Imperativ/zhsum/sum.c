#include "sum.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

static int indexof(int arr[], int item, int lastIndex) {
    int index = lastIndex + 1;
    for (int i = 0; i <= lastIndex; ++i) {
        if (arr[i] == item) {
            index = i;
        }
    }
    return index;
}

int SumOfTwoInts(int arr[], int item, int lastIndex, int *a, int *b) {
    int result = 0;
    int i = 0;

    while (!result && i <= lastIndex) {
        int difference = item - arr[i];
        int idx = indexof(arr, difference, lastIndex);

        if (idx <= lastIndex && i != idx) {
            *a = i;
            *b = idx;
            result = 1;
        } else {
            ++i;
        }
    }
    return result;
}