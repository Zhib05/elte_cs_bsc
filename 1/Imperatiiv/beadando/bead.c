#include "bead.h"
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

void print_color(color c) {
    switch (c) {
        case BLACK:    printf("%s ", BG_BLACK); break;
        case RED:      printf("%s ", BG_RED); break;
        case GREEN:    printf("%s ", BG_GREEN); break;
        case YELLOW:   printf("%s ", BG_YELLOW); break;
        case BLUE:     printf("%s ", BG_BLUE); break;
        case MAGENTA:  printf("%s ", BG_MAGENTA); break;
        case CYAN:     printf("%s ", BG_CYAN); break;
        case WHITE:    printf("%s ", BG_WHITE); break;
        default:       printf(" "); break;
    }
    printf("%s", RESET);
}

void Image_print(const Image *img) {
    for (int i = 0; i < img->height; ++i) {
        for (int j = 0; j < img->width; ++j) {
            print_color(img->matrix[i][j]);
        }
        printf("\n");
    }
}

Image *readfajl(const char *fajlnev) {
    FILE *file = fopen(fajlnev, "r");
    if (!file) {
        perror("HIBA a fajl megnyitasakor");
        return NULL;
    }

    Image *img = malloc(sizeof(Image));
    if (!img) {
        perror("HIBA a memoria foglalaskor");
        fclose(file);
        return NULL;
    }

    fscanf(file, "%d %d", &img->width, &img->height);

    if (img->height > MAX_SIZE || img->width > MAX_SIZE) {
        fprintf(stderr, "Tul nagy meret");
        free(img);
        fclose(file);
        return NULL;
    }

    img->matrix = malloc(img->height * sizeof(color *));
    for (int i = 0; i < img->height; ++i) {
        img->matrix[i] = malloc(img->width * sizeof(color));
    }

    for (int i = 0; i < img->height; ++i) {
        for (int j = 0; j < img->width; ++j) {
            int temp;
            fscanf(file, "%d", &temp);
            img->matrix[i][j] = (color)temp;
        }
    }
    fclose(file);
    return img;
}


void Image_free(Image *img) {
    for (int i = 0; i < img->height; ++i) {
        free(img->matrix[i]);
    }
    free(img->matrix);
    free(img);
}

void print_gif(const char *fajlnev) {
    Gif gif;
    int count = 10;

    for (int i = 0; i < count; ++i) {
        char fajlname[100];
        sprintf(fajlname, "%s.bg%d", fajlnev, i);

        gif.kepek[i] = readfajl(fajlname);
        if (!gif.kepek[i]) {
            fprintf(stderr, "Hiba a %s fajl beolvasasakor\n", fajlname);
            for (int j = 0; j < i; ++j) {
                Image_free(gif.kepek[i]);
            }
            return;
        }
    }

    for (int i = 0; i < count; ++i) {
        printf(TERMINAL_CLEAR);
        printf(TERMINAL_HOME);

        Image_print(gif.kepek[i]);
        usleep(500000);
    }

    for (int i = 0; i < count; ++i) {
        Image_free(gif.kepek[i]);
    }
}