#include "lab.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

void print_board(Labirintus *lab) {
    if (lab->matrix == NULL) {
        printf("Nincs megjelenitheto labirintus.\n");
        return;
    }

    for (int i = 0; i < lab->rows; ++i) {
        for (int j = 0; j < lab->cols; ++j) {
            printf("%c", lab->matrix[i][j]);
        }
        printf("\n");
    }
}

void creat_labirintus(Labirintus *lab) {
    printf("Adjuk meg a labirintus meretet (sorok, oszlopok, %d-%d): ", MIN_DIM, MAX_DIM);
    int r, c;
    scanf("%d %d", &r, &c);

    if (r < MIN_DIM || r > MAX_DIM || c < MIN_DIM || c > MAX_DIM) {
        printf("Hibas meret probald ujra.\n");
        return;
    }

    free_labirintus(lab);

    lab->rows = r;
    lab->cols = c;

    lab->matrix = (char **)malloc(lab->rows * sizeof(char *));
    for (int i = 0; i < lab->rows; ++i) {
        lab->matrix[i] = (char *)malloc((lab->cols + 1) * sizeof(char));
    }

    printf("Add meg a  labirintus szerkezetet soronkent\n");
    for (int i = 0; i < lab->rows; ++i) {
        printf("%d. sor (%d karakter): ", i + 1, lab->cols);
        fgets(lab->matrix[i], lab->cols + 2, stdin);

        lab->matrix[i][strcspn(lab->matrix[i], "\n")] = '\0';

        if (strlen(lab->matrix[i]) != lab->cols) {
            printf("Hibas hosszusag! A sor %d karakter hosszu kell, hogy legyen.\n", lab->cols);
            i--;
        }
    }
}

void save_labirintus(Labirintus *lab) {
    if (lab->matrix == NULL) {
        printf("Nincs mentheto labirintus.\n");
        return;
    }

    char fajlname[50];
    printf("Add meg a fajl nevet: ");
    scanf("%s", fajlname);

    FILE *fp = fopen(fajlname, "w");
    if (!fp) {
        printf("Nem sikerult a fajl megnyitasa.\n");
        return;
    }

    fprintf(fp, "%d %d", lab->rows, lab->cols);
    for (int i = 0; i < lab->rows; ++i) {
        fprintf(fp, "%s\n", lab->matrix[i]);
    }
    
    fclose(fp);
    printf("Labirintus elmentve: %s\n", fajlname);
}

void load_labirintus(Labirintus *lab) {
    char fajlname[50];
    printf("Add meg a fajl nevet: ");
    scanf("%s", fajlname);

    FILE *fp = fopen(fajlname, "r");
    if (!fp) {
        printf("Nem sikerult a fajl megnyitasa.\n");
        return;
    }

    free_labirintus(lab);

    fscanf(fp, "%d %d\n", &lab->rows, &lab->cols);

    lab->matrix = (char **)malloc(lab->rows * sizeof(char *));
    for (int i = 0; i < lab->rows; ++i) {
        lab->matrix[i] = (char *)malloc((lab->cols + 1) * sizeof(char));
        if (fgets(lab->matrix[i], lab->cols + 2, fp) == NULL) {
            printf("Hiba a fajl beolvasasa kor!\n");
            fclose(fp);
            return;
        }
        lab->matrix[i][strcspn(lab->matrix[i], "\n")] = '\0';
    }
    fclose(fp);
    printf("Labirintus betoltve: %s\n", fajlname);
}

void play_labirintus(Labirintus *lab) {
    if (lab->matrix == NULL) {
        printf("Nincs elindithato labirintus.\n");
        return;
    }

    int player_row, player_col;

    srand(time(NULL));
    do {
        player_row = rand() % lab->rows;
        player_col = rand() % lab->cols;
    } while (lab->matrix[player_row][player_col] != ' ');

    lab->matrix[player_row][player_col] = 'P';

    char command;
    while (1) {
        print_board(lab);

        printf("Mozgasd a jatekost: ");
        scanf(" %c", &command);

        lab->matrix[player_row][player_col] = ' ';

        int new_row = player_row;
        int new_col = player_col;

        switch (command) {
            case 'w': new_row--; break;
            case 's': new_row++; break;
            case 'a': new_col--; break;
            case 'd': new_col++; break;
            case 'r': 
                printf("Feladtad a jatekot. Vissza a menuhoz\n");
                lab->matrix[player_row][player_col] = 'P';
                return;
            default:
                printf("Ervenytelen parancs!\n");
                break;
        }

        if (new_row >= 0 && new_row < lab->rows && new_col >= 0 && new_col < lab->cols && lab->matrix[new_row][new_col] != '#') {
            player_row = new_row;
            player_col = new_col;
        } else {
            printf("Nem lephetsz ide!\n");
        }

        if ((player_row == 0 || player_row == lab->rows - 1 || player_col == 0 || player_col == lab->cols - 1) && lab->matrix[player_row][player_col] != '#') {
            printf("Gratulalok, kijutottal!\n");
            return;
        }

        lab->matrix[player_row][player_col] = 'P';
    }
}

void free_labirintus(Labirintus *lab) {
    if (lab->matrix != NULL) {
        for (int i = 0; i < lab->rows; ++i) {
            free(lab->matrix[i]); 
        }
        free(lab->matrix);
        lab->matrix = NULL;
    }
}