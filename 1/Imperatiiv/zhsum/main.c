#include "sum.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char *argv[]) {
    // int len = 5;
    // int arr[5] = {2, 5, -7, 6, 9};

    // int test[3] = {-2, 7, 12};

    // int a, b;
    // for (int i = 0; i < 3; ++i) {
    //     int result = SumOfTwoInts(arr, test[i], len - 1, &a, &b);
    //     if(result) {
    //         printf("%d: igen, %d = arr[%d] + arr[%d]\n", test[i], test[i], a, b);
    //     } else {
    //         printf("%d: nem\n", test[i]);
    //     }
    // }

    if (argc != 4) {
        fprintf(stderr, "Hasznalat: %s felbontas tomb_merete szam1:szam2:...\n", argv[0]);
        return 1;
    }

    int osszeg = atoi(argv[1]);
    int n = atoi(argv[2]);
    int *arr = malloc(n * (sizeof(int)));

    if (arr) {
        char *tokens = strtok(argv[3], ":");

        int i = 0;
        while (tokens != NULL) {
            arr[i] = atoi(tokens);
            tokens = strtok(NULL, ":");
            ++i;
        }

        if (i != n) {
            fprintf(stderr, "%ddb szamot kell megadni\n", n);
            free(arr);
            return 1;
        }

        int a, b;
        int result = SumOfTwoInts(arr, osszeg, n - 1, &a, &b);
        if(result) {
            printf("%d: igen, %d = arr[%d] + arr[%d] = (%d) + (%d)\n", osszeg, osszeg, a, b, arr[a], arr[b]);
        } else {
            printf("%d: nem\n", osszeg);
        }
    }
    free(arr);
    return 0;
}