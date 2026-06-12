#ifndef BEAD_H
#define BEAD_H

#define MAX_SIZE 30

#define RESET       "\033[0m"
#define BG_BLACK    "\033[40m"
#define BG_RED      "\033[41m"
#define BG_GREEN    "\033[42m"
#define BG_YELLOW   "\033[43m"
#define BG_BLUE     "\033[44m"
#define BG_MAGENTA  "\033[45m"
#define BG_CYAN     "\033[46m"
#define BG_WHITE    "\033[47m"
#define TERMINAL_CLEAR  "\033[2J" 
#define TERMINAL_HOME   "\033[2H" 

typedef enum {
    BLACK,
    RED,
    GREEN,
    YELLOW,
    BLUE,
    MAGENTA,
    CYAN,
    WHITE
} color;

typedef struct {
    int width;
    int height;
    color **matrix;
} Image;

typedef struct {
    Image *kepek[10];
} Gif;

void print_color(color c);
void Image_print(const Image *img);
Image *readfajl(const char *fajlnev);
void Image_free(Image *img);
void print_gif(const char *fajlnev);

#endif