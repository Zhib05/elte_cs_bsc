#include "vektor.h"
#include <stdio.h>
#include <stdlib.h>

int main() {
    Vektor v;

    initialize(&v, 5);

    append(&v, 1);
    append(&v, 2);
    append(&v, 3);
    append(&v, 4);

    printf("%d\n", retrieve(&v, 0));
    printf("%d\n", retrieve(&v, 1));
    printf("%d\n", retrieve(&v, 2));
    printf("%d\n", retrieve(&v, 3));
    putchar('\n');

    insert(&v, 4, 5);

    printf("%d\n", retrieve(&v, 0));
    printf("%d\n", retrieve(&v, 1));
    printf("%d\n", retrieve(&v, 2));
    printf("%d\n", retrieve(&v, 3));
    printf("%d\n", retrieve(&v, 4));
    putchar('\n');

    set_capacity(&v, 6);
    safe_append(&v, 6);
    safe_append(&v, 7);

    printf("%d\n", retrieve(&v, 0));
    printf("%d\n", retrieve(&v, 1));
    printf("%d\n", retrieve(&v, 2));
    printf("%d\n", retrieve(&v, 3));
    printf("%d\n", retrieve(&v, 4));
    printf("%d\n", retrieve(&v, 5));
    printf("%d\n", retrieve(&v, 6));
    putchar('\n');

    safe_insert(&v, 0, 0);

    printf("%d\n", retrieve(&v, 0));
    printf("%d\n", retrieve(&v, 1));
    printf("%d\n", retrieve(&v, 2));
    printf("%d\n", retrieve(&v, 3));
    printf("%d\n", retrieve(&v, 4));
    printf("%d\n", retrieve(&v, 5));
    printf("%d\n", retrieve(&v, 6));
    printf("%d\n", retrieve(&v, 7));
    putchar('\n');

    erase(&v, 0);

    printf("%d\n", retrieve(&v, 0));
    printf("%d\n", retrieve(&v, 1));
    printf("%d\n", retrieve(&v, 2));
    printf("%d\n", retrieve(&v, 3));
    printf("%d\n", retrieve(&v, 4));
    printf("%d\n", retrieve(&v, 5));
    printf("%d\n", retrieve(&v, 6));
    putchar('\n');

    dispose(&v);

    return 0;
}