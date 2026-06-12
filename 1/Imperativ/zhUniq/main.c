#include "uniq.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char *argv[]) {
    FILE *file;

    int size = 4;
    int count = 0;

    Uniq *uniq = malloc(size * sizeof(Uniq));
    if (!uniq) {
        perror("Hiba a memoria foglalasa kor\n");
        return 1;
    }

    int mode;
    if (argc == 1) {
        mode = 0;
    }
    else {
        mode = 1;
    }

    for (int i = 0; i < argc; ++i) {
        switch (mode) {
            case 0: 
                file = stdin; 
                break;
            case 1: 
                if (i == 0) {
                    continue;
                }
                else {
                    file = fopen(argv[i], "r");
                    break;
                }
            default: break;
        }

        if (!file) {
            fprintf(stderr, "Hiba a fajl megnyitasa kor!\n");
            continue;
        } 
        else {
            char buffer[1024];
            while (fgets(buffer, 1023, file) != NULL) {
                if (size == count) {
                    size *= 2;
                    uniq = realloc(uniq, size * sizeof(Uniq));
                    if (!uniq) {
                        perror("Hiba a memoria foglalasa kor\n");
                        return 1;
                    }
                }

                buffer[1023] = '\0';
                buffer[strcspn(buffer, "\r\n")] = 0;

                int notIn = 1;

                for (int i = 0; i < count; ++i) {
                    if (!(strcasecmp(buffer, uniq[i].strings))) {
                        uniq[i].cnt++;
                        notIn = 0;
                    }
                }

                if (notIn) {
                    strcpy(uniq[count].strings, buffer);
                    uniq[count].cnt = 0;
                    uniq[count].cnt++;
                    count++;
                }
            }
            print_sort(uniq, count);
            reset(uniq, count);
            fclose(file);
        }
    }


    free(uniq);

    return 0;
}