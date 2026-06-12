#include "lab.h"
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>

int main() {
    int choice;
    Labirintus lab = {NULL, 0, 0};
    srand(time(NULL));

    while (1) {
        puts("Udvozollek a labirintus jatekban!");
        puts("A jatek celja a kijarat elerese");
        puts("A jatekos a w-felfele, a-balra, s-lefele, d-jobbra lejutesevel lehet mozgatni");
        puts("########################");
        puts("###   1.kirazolas    ###");
        puts("###   2.keszites     ###");
        puts("###   3.mentes       ###");
        puts("###   4.betoltes     ###");
        puts("###   5.jatek        ###");
        puts("###   6.kilépés      ###");
        puts("########################");
        scanf("%d", &choice);

        switch (choice) {
            case 1:
                print_board(&lab);
                break;
            case 2:
                creat_labirintus(&lab);
                break;
            case 3:
                save_labirintus(&lab);
                break;
            case 4:
                load_labirintus(&lab);
                break;
            case 5:
                play_labirintus(&lab);
                break;
            case 6:
                printf("kilepes...\n");
                free_labirintus(&lab);
                return 0;
            default:
                printf("Ervenytelen opcio. Probald ujra!\n");
        }
    }
}