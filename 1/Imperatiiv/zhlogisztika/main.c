#include "log.h"
#include <stdio.h>
#include <stdlib.h>

int main() {
    Node* root = NULL;

    int arr[] = {8, 10, 3, 14, 1, 13, 6, 4, 7};
    int n = sizeof(arr) / sizeof(arr[0]);
    for (int i = 0; i < n; ++i) {
        root = insert_pkg(root, arr[i]);
    }

    int command, distance;
    scanf("%d", &command);

    do {
        printf("Udvozlom a logsztika programban!\n");
        printf("Az alabbi funkciok kozul valaszthatsz.\n");
        printf("1) Hozzaadas\n");
        printf("2) kirajzolas\n");
        printf("3) rakodasi lista\n");
        printf("4) csomag torlese\n");
        printf("5) kilepes\n");

        switch (command) {
            case 1: 
                printf("Add meg a depotol valo tavolsagot\n");
                scanf("%d", &distance);
                insert_pkg(root, distance); 
                break;
            case 2:
                print_tree(root, 0);
        }
    } while (command != 5);

    return 0;
}