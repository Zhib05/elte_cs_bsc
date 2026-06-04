#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct {
    char strings[1024];
    int cnt;
} Uniq;

void process_file(FILE *file, Uniq **uniq, int *size, int *count) {
    char buffer[1024];

    while (fgets(buffer, 1023, file) != NULL) {
        if (*size == *count) {
            *size *= 2;
            *uniq = realloc(*uniq, (*size) * sizeof(Uniq));
            if (!*uniq) {
                perror("Hiba a memoria foglalasa kor\n");
                exit(1);
            }
        }

        buffer[1023] = '\0';
        buffer[strcspn(buffer, "\r\n")] = 0;

        int notIn = 1;

        for (int i = 0; i < *count; ++i) {
            if (!(strcasecmp(buffer, (*uniq)[i].strings))) {
                (*uniq)[i].cnt++;
                notIn = 0;
                break;
            }
        }

        if (notIn) {
            strcpy((*uniq)[*count].strings, buffer);
            (*uniq)[*count].cnt = 1;
            (*count)++;
        }
    }
}

void print_sort(Uniq *uniq, int count) {
    int *array = malloc(4 * sizeof(int));
    int *index = malloc(4 * sizeof(int));

    for (int i = 0; i < count; ++i) {
        array[i] = uniq[i].cnt;
        index[i] = i;
    }

    int temp, temp2;
    for (int i = 0; i < count; ++i) {
        for (int j = i + 1; j < count; ++j) {
            if (array[i] < array[j]) {
                temp = array[i];
                temp2 = index[i];
                array[i] = array[j];
                index[i] = index[j];
                array[j] = temp;
                index[j] = temp2;
            }
        }
    }

    for (int i = 0; i < count; ++i) {
        printf("%d %s\n", uniq[index[i]].cnt, uniq[index[i]].strings);
    }

    free(array);
    free(index);
}

int main(int argc, char *argv[]) {
    FILE *file;

    int size = 4;
    int count = 0;

    Uniq *uniq = malloc(size * sizeof(Uniq));
    if (!uniq) {
        perror("Hiba a memoria foglalasa kor\n");
        return 1;
    }

    int mode = (argc == 1) ? 0 : 1;

    for (int i = 0; i < argc; ++i) {
        switch (mode) {
            case 0:
                file = stdin;
                break;
            case 1:
                if (i == 0) {
                    continue;
                } else {
                    file = fopen(argv[i], "r");
                    if (!file) {
                        fprintf(stderr, "Hiba a fajl megnyitasa kor!\n");
                        continue;
                    }
                }
                break;
            default:
                break;
        }

        if (file) {
            process_file(file, &uniq, &size, &count);
            if (mode == 1) {
                fclose(file);
            }
        }
    }

    print_sort(uniq, count);
    free(uniq);

    return 0;
}