#include <stdlib.h>
#include <stdio.h>
#include <time.h>
#include <string.h>

#define APPLE_COUNT 10
#define INITIAL_SNAKE_LENGTH 9

typedef enum {
    UPDATE_COLLIDED_WITH_SELF = -2,
    UPDATE_COLLIDED_WITH_WALL = -1,
    UPDATE_NONE = 0,
    UPDATE_COLLECTED_APPLE = 1,
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
/* 
 * SEGÉDFÜGGVÉNYEK
 *
 * (opcionális)
 * Mivel csak ebben a fájlban használjuk, a static kulcsszóval elrejthetjük őket.
 */

static void init_field(char **fields, int rows, int cols) {
    for (int i = 0; i < rows; ++i)
        for (int j = 0; j < cols; ++j)
            fields[i][j] = ' ';

    int placed = 0;
    while (placed < APPLE_COUNT) {
        int index = rand() % (rows * cols);
        int x = index / cols;
        int y = index % cols;

        /* Ha már van ott alma, ne próbáljunk még egyet letenni */
        if (fields[x][y] != 'a') {
            fields[x][y] = 'a';
            ++placed;
        }
    }
}

static void init_snake(Point snake[INITIAL_SNAKE_LENGTH]) {
    for (int i = 0; i < INITIAL_SNAKE_LENGTH; ++i) {
        snake[i].x = 0;
        snake[i].y = INITIAL_SNAKE_LENGTH - i - 1;
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

    /*
     * Mátrix kétfajta tárolása:
     * - tömb, amiben sorokra (amik maguk is tömbök) mutató pointerek vannak
     *   ha ilyet csinálunk, fontos: felszabadításnál a "belső" tömböket is free()-eljük!
     * - egyetlen tömb, sorfolytonosan; ekkor az indexelés bonyolultabb (órán néztük)
     */
    char **fields = malloc(rows * sizeof(char*));
    for (int i = 0; i < cols; ++i)
        fields[i] = malloc(cols * sizeof(char));
    state.fields = fields;
    init_field(fields, rows, cols);

    Point *snake = malloc(INITIAL_SNAKE_LENGTH * sizeof(Point));
    state.snake = snake;
    init_snake(snake);

    state.snake_length = INITIAL_SNAKE_LENGTH;

    return state;
}

void print_game(State *state) {
    /* 
     * Másolat készítése a játékmátrixról a kirajzoláshoz.
     * Nem elég csak a sorokra mutató pointereket átmásolni,
     * hanem a sorokról is másolatot kell készíteni.
     */
    char **display = malloc(state->rows * sizeof(char*));
    for (int i = 0; i < state->rows; ++i) {
        display[i] = malloc(state->cols * sizeof(char));
        for (int j = 0; j < state->cols; ++j)
            display[i][j] = state->fields[i][j];
    }
    
    Point head = state->snake[0];
    display[head.x][head.y] = '8';
    for (int i = 1; i < state->snake_length; ++i) {
        Point segment = state->snake[i];
        display[segment.x][segment.y] = '0';
    }

    print_border(state->cols + 2);
    
    for (int i = 0; i < state->rows; ++i) {
        putchar('#');

        for (int j = 0; j < state->cols; ++j) {
            putchar(display[i][j]);
        }

        putchar('#');
        putchar('\n');
    }

    print_border(state->cols + 2);

    /* Ne felejtsük el a sorokat is felszabadítani! */
    for (int i = 0; i < state->rows; ++i)
        free(display[i]);
    free(display);
}

int update_snake(State *state, char direction) {
    Point head = state->snake[0];
    switch (direction) {
        case 'w': head.x--; break;
        case 's': head.x++; break;
        case 'a': head.y--; break;
        case 'd': head.y++; break;
    }

    /* Ha a mozgással a pályán kívül esne a feje */
    if (head.x < 0 || head.y < 0)
        return UPDATE_COLLIDED_WITH_WALL;
    if (head.x >= state->rows || head.y >= state->cols)
        return UPDATE_COLLIDED_WITH_WALL;
    
    /* Ha a feje olyan pozícióra került, ahol a teste van */
    for (int i = 1; i < state->snake_length; ++i)
        if (head.x == state->snake[i].x && head.y == state->snake[i].y)
            return UPDATE_COLLIDED_WITH_SELF;
    
    if (state->fields[head.x][head.y] == ' ') {
        /*
         * Nem volt ott alma -> a szegmenseket eltoljuk
         * Ahol eddig a feje volt, most a második szegmens lesz az, stb.
         */
        for (int i = state->snake_length - 1; i >= 1; --i)
            state->snake[i] = state->snake[i - 1];

        state->snake[0] = head;

        return UPDATE_NONE;
    } else {
        /* Volt alma - megnöveljük a kígyót */
        Point *snake = malloc(sizeof(Point) * (state->snake_length + 1));
        snake[0] = head;
        for (int i = 1; i < state->snake_length + 1; ++i)
            snake[i] = state->snake[i - 1];

        state->snake_length++;
        free(state->snake);
        state->snake = snake;

        /* Töröljük az almát, hogy ne gyűjtsünk be még egyet, ha újra ideérünk  */
        state->fields[head.x][head.y] = ' ';

        return UPDATE_COLLECTED_APPLE;
    }
}

void free_game(State *state) {
    for (int i = 0; i < state->rows; ++i)
        free(state->fields[i]);
    free(state->fields);
    free(state->snake);
}

int main(int argc, char *argv[]) {
    /* Véletlen számokat fogunk használni, inicializáljuk a random generátort! */
    srand(time(NULL));

    if (argc <= 2) {
        fprintf(stderr, "Usage: %s <height> <width>\n", argv[0]);
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

    /* 
     * Ezt csak azért tettem ide, mert az init_snake() függvényem az első sorba
     * próbálja az egész kígyót lehelyezni. Lehetne más megoldás is (pl. csigavonal
     * rajzolása vagy a kígyó rövidítése) túl keskeny pálya esetéén, ZH-n le lesz írva,
     * mit kell tenni, ha ilyen lesz.
     */
    if (width < 9) {
        fprintf(stderr, "Width must be at least 9\n");
        return 1;
    }

    State state = init_game(height, width);
    print_game(&state);

    /* 
     * Egyesével megyünk végig a beolvasott karaktereken, de attól még csak enter
     * lenyomására olvasódik be.
     */
    int command;
    int apples_remaining = APPLE_COUNT;
    while ((command = getchar()) != EOF) {
        if (command != 'w' && command != 'a' && command != 's' && command != 'd') continue;

        UpdateResult res = update_snake(&state, command);
        if (res < 0) {
            /* A "hiba" állapotok negatív értéket kaptak */
            printf("Utkoztel %s. A jatek leall.", (res == UPDATE_COLLIDED_WITH_SELF) ? "onmagaddal" : "a fallal");
            break; /* return 0 nem jó itt, mert a free_game() ekkor nem hívódna meg */
        }

        print_game(&state);
        if (res == UPDATE_COLLECTED_APPLE) {
            apples_remaining--;
            if (apples_remaining == 0) {
                puts("Gratulalok, osszes almát begyujtotted!");
                break;
            }
        }
    }

    free_game(&state);

    return 0;
}