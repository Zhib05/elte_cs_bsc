#include "uniq.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void reset(Uniq* arr, int cnt) {
    for (int j=0; j<cnt; j++) {
        arr[j].cnt = 0;
        strcpy(arr[j].strings,"");
    }
}

void print_sort(Uniq *uniq, int count) {
    int *array = malloc(4 * sizeof(int));
    int *index = malloc(4 * sizeof(int));

    for (int i = 0; i < count; ++i) {
        array[i] = uniq[i].cnt;
        index[i] = i;
    }

    int temp, temp2;
    for (int i = 0; i < count; ++i) {
        for (int j = i + 1; j < count; ++j) {
            if (array[i] < array[j]) {
                temp = array[i];
                temp2 = index[i];
                array[i] = array[j];
                index[i] = index[j];
                array[j] = temp;
                index[j] = temp2;
            }
        }
    }

    for (int i = 0; i < count; ++i) {
        printf("%d %s\n", uniq[index[i]].cnt, uniq[index[i]].strings);
    }

    free(array);
    free(index);
}