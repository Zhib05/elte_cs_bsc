#include "zh.h"
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>

int main(int argc, char *argv[]) {
    srand(time(NULL));

    if (argc <= 2) {
        fprintf(stderr, "hasznalat: %s <heigth> <width>\n", argv[0]);
        return 1;
    }

    puts("Udvozollek a kigyo jatekban");
    puts("A jatek celja a 10 alma (melyeket az \'a\' karakter jelol) begyujtese.");
    puts("A kigyo fejet a \'8\' karakter, a testet \'0\' jeloli. Mozgatashoz hasznald az alabbi karaktereket:");
    puts("- a: balra");
    puts("- s: le");
    puts("- d: jobbra");
    puts("- w: fel");
    puts("Egyszerre tobb lepest is megadhatsz, de csak az <ENTER> leutese utan hajtodnak vegre.");
    puts("Vigyazz! Ha a kigyo onmagaba vagy a falba (\'#\') utkozik, a jatek veget er.");

    int height = atoi(argv[1]);
    int width = atoi(argv[2]);

    if (width < 9) {
        fprintf(stderr, "Az oszlopok szama legalabb 9\n");
        return 1;
    }

    State state = init_game(height, width);
    print_game(&state);
    
    int command;
    int maradek_alma = APPLE;
    while ((command = getchar()) != EOF) {
        if (command != 'w' && command != 'a' && command != 's' && command != 'd') continue;

        UpdateResult res = update_snake(&state, command);
        if (res < 0) {
            printf("utkoztel %s. A jatek vege.\n", (res == utkozes_fallal) ? "a fallal" : "onmagaddal");
            break;
        }

        print_game(&state);
        if (res == alma_gyujtes) {
            --maradek_alma;
            if (maradek_alma == 0) {
                puts("gratulalok felszetted az osszes almat!\n");
                break;
            }
        }
    }

    for (int i = 0; i < state.rows; ++i)  // Memóriafelszabadítás
        free(state.fields[i]);
    free(state.fields);
    free(state.snake);

    return 0;
}