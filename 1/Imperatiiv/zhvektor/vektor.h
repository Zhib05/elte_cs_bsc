#ifndef VEKTOR_H
#define VEKTOR_H

#define ELEM int

typedef struct {
    unsigned int capacity;
    unsigned int curr_size;
    ELEM *elements;
} Vektor;

int initialize(Vektor *v, int capacity);
void dispose(Vektor *v);
int append(Vektor *v, ELEM e);
ELEM retrieve(Vektor *v, unsigned int index);
int insert(Vektor *v, unsigned int index, ELEM e);
int erase(Vektor *v, unsigned int index);
int set_capacity(Vektor *v, int c);
int safe_append(Vektor *v, ELEM e);
int safe_insert(Vektor *v, unsigned int index, ELEM e);

#endif