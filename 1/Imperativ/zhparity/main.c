#include "parity.h"
#include <stdio.h>
#include <stdlib.h>

int main() {
    Pa arr;
    InitParityArray(&arr, 5);

    InsertToParityArray(&arr, 2);
    InsertToParityArray(&arr, 7);
    InsertToParityArray(&arr, 1);

    PrintParityArray(&arr);

    putchar('\n');

    InsertToParityArray(&arr, 9);
    InsertToParityArray(&arr, 4);

    PrintParityArray(&arr);

    DisposeParityArray(&arr);
    return 0;
}