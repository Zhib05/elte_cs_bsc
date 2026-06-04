#include "bead.h"
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

int main(int argc, char *argv[]) {
    if (argc != 2) {
        fprintf(stderr, "Hasznalat: %s <fajlnev>\n", argv[0]);
        return 1;
    }

    const char *fajlnev = argv[1];

    print_gif(fajlnev);
    return 0;
}