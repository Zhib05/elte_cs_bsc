#ifndef UNIQ_H
#define UNIQ_H

typedef struct {
    char strings[1024];
    int cnt;
} Uniq;

void reset(Uniq* arr, int cnt);
void print_sort(Uniq *uniq, int count);

#endif