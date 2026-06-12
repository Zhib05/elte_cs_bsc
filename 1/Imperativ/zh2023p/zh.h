#ifndef ZH_H
#define ZH_H

#define APPLE 10
#define SNAKE_LENGTH 9

typedef enum {
    utkozes_onmagaval = -2,
    utkozes_fallal = -1,
    sikeres_mozgas = 0,
    alma_gyujtes = 1,
} UpdateResult;

typedef struct {
    int x, y;
} Point;

typedef struct {
    int rows, cols;
    char **fields;
    Point *snake;
    int snake_length;
} State;

UpdateResult update_snake(State *state, char direction);
// void init_field(char **fields, int rows, int cols);
State init_game(int rows, int cols);
// void init_snake(Point snake[SNAKE_LENGTH]);
// void print_border(int cnt);
void print_game(State *state);

#endif