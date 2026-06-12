#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_WORDS 5
#define MAX_LENGTH 100 // Feltételezhető maximális szóméret

int main() {
    char *words[MAX_WORDS]; // Pointer tömb a szavak tárolásához
    int i;

    printf("Adj meg %d szót:\n", MAX_WORDS);

    // Szavak beolvasása
    for (i = 0; i < MAX_WORDS; i++) {
        char temp[MAX_LENGTH];
        printf("Szó %d: ", i + 1);
        scanf("%s", temp);

        // Memória foglalása a szó hosszának megfelelően
        words[i] = (char *)malloc(strlen(temp) + 1); // +1 a null-terminátor miatt
        if (words[i] == NULL) {
            perror("Memória foglalási hiba");
            exit(1);
        }

        // A szöveg másolása a dinamikusan foglalt memóriába
        strcpy(words[i], temp);
    }

    // Szavak kiírása fordított sorrendben
    printf("\nFordított sorrend:\n");
    for (i = MAX_WORDS - 1; i >= 0; i--) {
        printf("%s\n", words[i]);
        free(words[i]); // A memória felszabadítása
    }

    return 0;
}
