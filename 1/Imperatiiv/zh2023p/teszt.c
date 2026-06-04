#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>

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

void init_field(char **fields, int rows, int cols) {
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

void init_snake(Point snake[SNAKE_LENGTH]) {
    for (int i = 0; i < SNAKE_LENGTH; ++i) {
        snake[i].x = 0;
        snake[i].y = SNAKE_LENGTH - i - 1;
    }
}

void print_border(int cnt) {
    for (int i = 0; i < cnt; ++i)
        putchar('#');
    putchar('\n');
}

State init_game(int rows, int cols) {
    State state;
    state.rows = rows;
    state.cols = cols;

    char **fields = malloc(rows * sizeof(char *));
    for(int i = 0; i < cols; ++i)
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
    for (int i = 0; i < state->cols; ++i) {
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
        for (int i = SNAKE_LENGTH - 1; i >= 1; ++i)
            state->snake[i] = state->snake[i - 1];
        state->snake[0] = head;
        
        return sikeres_mozgas;
    }
    else {
        Point *snake = malloc(sizeof(Point) * (state->snake_length + 1));
        snake[0] = head;
        for (int i = 1; i < SNAKE_LENGTH + 1; ++i)
            snake[i] = state->snake[i - 1];
        
        ++state->snake_length;
        free(state->snake);
        state->snake = snake;
    }
}

int main(int argc, char *argv[]) {
    srand(time(NULL));

    if (argc <= 2) {
        fprintf(stderr, "hasznalat: %s <heigth> <width>", argv[0]);
        return 1;
    }

    puts("Udvozollek a kigyo jatekban");
    puts("A jatek celja a 10 alma (melyeket az \'a\' karakter jelol) begyujtese.");
    puts("A kigyo fejet a \'8\' karakter, a testet \'0\' jeloli. Mozgatashoz hasznald az alabbi karaktereket:");
    puts("- a: balra");
    puts("- s: le");
    puts("- d: jobbra");
    puts("- w: fel");
    puts("Egyszerre tobb lepest is megadhatsz, de csak az <ENTER> leutese utan hajtodnak vegre.");
    puts("Vigyazz! Ha a kigyo onmagaba vagy a falba (\'#\') utkozik, a jatek veget er.");

    int height = atoi(argv[1]);
    int width = atoi(argv[2]);

    if (width < 9) {
        fprintf(stderr, "Az oszlopok szama legalabb 9\n");
        return 1;
    }

    State state = init_game(height, width);
    print_game(&state);
    
    char command = getchar();
    int maradek_alma = APPLE;
    while (command != EOF) {
        if (command != 'w' && command != 'a' && command != 's' && command != 'd') continue;

        UpdateResult res = update_snake(&state, command);
        if (res < 0) {
            printf("utkoztel %s. A jatek vege.\n", (res == utkozes_fallal) ? "a fallal" : "onmagaddal");
            break;
        }

        print_game(&state);
        if (res == alma_gyujtes) {
            --maradek_alma;
            if (maradek_alma == 0) {
                puts("gratulalok felszetted az osszes almat!\n");
                break;
            }
        }
    }

    return 0;
}