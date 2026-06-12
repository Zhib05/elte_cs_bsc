#include "domino.h"
#include <stdio.h>
#include <stdlib.h>

void initArray(Domino **arr, int count) {
    for (int i = 0; i < count; ++i) {
        arr[i] = malloc(sizeof(Domino));
        if (!arr[i]) {
            fprintf(stderr, "Hiba a memoria foglalasa kor!\n");
            exit(1);
        }
    }
}

int place(Domino *dp, int *setLeft, int *setRight) {
     if (dp->right == *setRight) {
        *setRight = dp->left;
        printf("Added to the Right end: %d|%d\n", dp->left, dp->right);
    } else if (dp->left == *setRight) {
        *setRight = dp->right;
        printf("Added to the Right end: %d|%d\n", dp->left, dp->right);
    } else if (dp->right == *setLeft) {
        *setLeft = dp->left;
        printf("Added to the Left end:  %d|%d\n", dp->left, dp->right);
    } else if (dp->left == *setLeft) {
        *setLeft = dp->right;
        printf("Added to the Left end:  %d|%d\n", dp->left, dp->right);
    } else {
        return 0;
    }
    
    return 1;
}

void freeArray(Domino **arr, int count) {
    for (int i = 0; i < count; ++i) {
        if(arr[i]) {
            free(arr[i]);
        }
    }
    free(arr);
}

int main() {
    FILE *fp = fopen("input02.txt", "r");
    if (!fp) {
        fprintf(stderr, "Hiba a fajl megnyitasa kor!\n");
        return 1;
    }

    int setLeft;
    fscanf(fp, "%d", &setLeft);

    int setRight;
    fscanf(fp, "%d", &setRight);

    int count;
    fscanf(fp, "%d", &count);

    Domino **arr = malloc(count * sizeof(Domino *));
    if (arr) {
        initArray(arr, count);
        for(int i = 0; i < count; ++i) {
            int l, r;
            if (fscanf(fp, "%d%d", &l, &r) != 2) {
                fprintf(stderr, "Hiba az input beolvasasa kozben!\n");
                freeArray(arr, count);
                fclose(fp);
                return 1;
            }

            arr[i]->left = l;
            arr[i]->right = r;
        }
        fclose(fp);

        printf("Initial domino: %d|%d\n", setLeft, setRight);

        int placed = 0;
        for (int i = 0; i < count; ++i) {
            for (int j = 0; j < count; ++j) {
                if (arr[j] && place(arr[j], &setLeft, &setRight)) {
                    placed++;
                    free(arr[j]);
                    arr[j] = NULL;
                    break;
                }
            }
        }

        printf("Summary: %d dominos were placed\n", placed);

        freeArray(arr, count);

        return 0;
    } else {
        fprintf(stderr, "Memory allocation failed\n");
        return 1;
    }
}