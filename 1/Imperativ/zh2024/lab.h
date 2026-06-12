#ifndef LAB_H
#define LAB_H

#define MAX_DIM 10
#define MIN_DIM 5

typedef struct {
    char **matrix;
    int rows;
    int cols;
} Labirintus;

void print_board(Labirintus *lab);
void creat_labirintus(Labirintus *lab);
void free_labirintus(Labirintus *lab);
void save_labirintus(Labirintus *lab);
void load_labirintus(Labirintus *lab);
void play_labirintus(Labirintus *lab);

#endif