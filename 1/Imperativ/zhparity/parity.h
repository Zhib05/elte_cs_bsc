#ifndef PARITY_H
#define PARITY_H

#define MAX_ARRAY_SIZE 5

typedef struct {
    int *arr;
    int even_idx;
    int odd_idx;
    unsigned int size;
} Pa;

void PrintParityArray(Pa *array);
int InsertToParityArray(Pa *array, int item);
int InitParityArray(Pa *array, int size);
void DisposeParityArray(Pa *array);

#endif