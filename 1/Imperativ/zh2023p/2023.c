#include "zh.h"
#include <stdio.h>
#include <stdlib.h>

static void init_field(char **fields, int rows, int cols) {
    for (int i = 0; i < rows; ++i)
        for (int j = 0; j < cols; ++j)
            fields[i][j] = ' ';
    
    int placed = 0;
    while (placed < APPLE) {
        int index = rand() % (rows * cols);
        int x = index / cols;
        int y = index % cols;

        if (fields[x][y] != 'a') {
            fields[x][y] = 'a';
            ++placed;
        }
    }
}

static void init_snake(Point snake[SNAKE_LENGTH]) {
    for (int i = 0; i < SNAKE_LENGTH; ++i) {
        snake[i].x = 0;
        snake[i].y = SNAKE_LENGTH - i - 1;
    }
}

static void print_border(int cnt) {
    for (int i = 0; i < cnt; ++i)
        putchar('#');
    putchar('\n');
}

State init_game(int rows, int cols) {
    State state;
    state.rows = rows;
    state.cols = cols;

    char **fields = malloc(rows * sizeof(char *));
    for(int i = 0; i < rows; ++i)
        fields[i] = malloc(cols * sizeof(char));
    state.fields = fields;
    init_field(fields, rows, cols);

    Point *snake = malloc(SNAKE_LENGTH * sizeof(Point));
    state.snake = snake;
    init_snake(snake);

    state.snake_length = SNAKE_LENGTH;

    return state;
}

void print_game(State *state) {
    char **munka = malloc(state->rows * sizeof(char *));
    for (int i = 0; i < state->rows; ++i) {
        munka[i] = malloc(state->cols * sizeof(char));
        for (int j = 0; j < state->cols; ++j)
            munka[i][j] = state->fields[i][j];
    }
    Point head = state->snake[0];
    munka[head.x][head.y] = '8';
    for (int i = 1; i < SNAKE_LENGTH; ++i) {
        Point test = state->snake[i];
        munka[test.x][test.y] = '0';
    }

    print_border(state->cols + 2);

    for (int i = 0; i < state->rows; ++i) {
        putchar('#');

        for (int j = 0; j < state->cols; ++j)
            putchar(munka[i][j]);
        
        putchar('#');
        putchar('\n');
    }

    print_border(state->cols + 2);

    for (int i = 0; i < state->rows; ++i)
        free(munka[i]);
    free(munka);
}

int update_snake(State *state, char direction) {
    Point head = state->snake[0];
    switch (direction) {
        case 'w': --head.x; break;
        case 'a': --head.y; break;
        case 's': ++head.x; break;
        case 'd': ++head.y; break;
    }

    if (head.x < 0 || head.y < 0)
        return utkozes_fallal;
    if (head.x >= state->rows || head.y >= state->cols)
        return utkozes_fallal;

    for (int i = 1; i < state->snake_length; ++i) {
        if (head.x == state->snake[i].x && head.y == state->snake[i].y)
            return utkozes_onmagaval;
    }

    if (state->fields[head.x][head.y] == ' ') {
        for (int i = state->snake_length - 1; i >= 1; --i)
            state->snake[i] = state->snake[i - 1];
        state->snake[0] = head;
        
        return sikeres_mozgas;
    }
    else {
        Point *snake = malloc(sizeof(Point) * (state->snake_length + 1));
        snake[0] = head;
        for (int i = 1; i < state->snake_length + 1; ++i)
            snake[i] = state->snake[i - 1];
        
        state->snake_length++;
        free(state->snake);
        state->snake = snake;

        state->fields[head.x][head.y] = ' ';

        return alma_gyujtes;
    }
}