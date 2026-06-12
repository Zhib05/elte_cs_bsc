#include "vektor.h"
#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[]) {
    if (argc > 1) {
        int capacity = atoi(argv[1]);
        Vektor v;
        initialize(&v, capacity);

        for (int i = 2; i < argc; i += 2) {
            switch (argv[i][0]) {
                case 'a' : 
                    printf("a : %d\n", append(&v, atoi(argv[i + 1])));
                    break;
                case 'r' :
                    printf("r : %d\n", retrieve(&v, atoi(argv[i + 1])));
                    break;
            }
        }
        dispose(&v);
    }

    return 0;
}