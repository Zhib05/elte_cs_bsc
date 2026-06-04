#include "parity.h"
#include <stdio.h>
#include <stdlib.h>

int InitParityArray(Pa *array, int size) {
    array->even_idx = 0;
    array->odd_idx = size - 1;
    array->size = size;
    array->arr = calloc(size, sizeof(int));
    if (array->arr) {
        for (int i = 0; i < size; ++i) {
            if (array->arr[i] == 0) {
                array->arr[i] = -1;
            }
        }
        return 1;
    }

    return 0;
}

void PrintParityArray(Pa *array) {
    for (int i = 0; i < (int)array->size; ++i) {
        printf("%d ", array->arr[i]);
        putchar('\n');
    }
}

int InsertToParityArray(Pa *array, int item) {
    if (array->even_idx > array->odd_idx) {
        return 1;
    }

    if (item % 2 == 0) {
        array->arr[array->even_idx] = item;
        array->even_idx++;
    } else {
        array->arr[array->odd_idx] = item;
        array->odd_idx--;
    }
    return 0;
}

void DisposeParityArray(Pa *array) {
    array->even_idx = 0;
    array->odd_idx = 0;
    array->size = 0;
    free(array->arr);
    array->arr = NULL;
}