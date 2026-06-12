#include "vektor.h"
#include <stdio.h>
#include <stdlib.h>

int initialize(Vektor *v, int capacity) {
    if (capacity > 0) {
        v->elements = malloc(capacity * sizeof(int));
        if (v->elements) {
            v->capacity = capacity;
            v->curr_size = 0;
            return 1;
        }
    }

    return 0;
}

void dispose(Vektor *v) {
    free(v->elements);
    v->capacity = 0;
    v->curr_size = 0;
}

int append(Vektor *v, ELEM e) {
    if(v->curr_size < v->capacity) {
        v->elements[v->curr_size] = e;
        v->curr_size++;

        return 1;
    }

    return 0;
}

ELEM retrieve(Vektor *v, unsigned int index) {
    return v->elements[index];
}

int insert(Vektor *v, unsigned int index, ELEM e) {
    if (v->capacity > v->curr_size && index <= v->curr_size) {
        for (int i = v->curr_size + 1; i > index; --i) {
            v->elements[i] = v->elements[i - 1];
        }
        v->elements[index] = e;
        v->curr_size++;

        return 1;
    }

    return 0;
}

int erase(Vektor *v, unsigned int index) {
    if (index <= v->curr_size) {
        for (int i = index; i < v->curr_size; ++i) {
            v->elements[i] = v->elements[i + 1];
        }
        v->curr_size--;
        return 1;
    }
    return 0;
}

static void move_elements(Vektor *v, ELEM *new) {
    for (int i = 0; i < v->curr_size; ++i) {
        new[i] = v->elements[i];
    }
}

int set_capacity(Vektor *v, int c) {
    if (c >= v->capacity) {
        v->capacity = c;
        ELEM *new = malloc(c * sizeof(ELEM));
        move_elements(v, new);

        // free(v->elements);
        v->elements = new;
        return 1;
    }
    return 0;
}

int safe_append(Vektor *v, ELEM e) {
    if(v->curr_size < v->capacity) {
        if (append(v, e))
            return 1;
    }
    else if (v->curr_size == v->capacity) {
        if (set_capacity(v, (v->capacity) * 2)) {
            if (append(v, e))
                return 1;
        }
    }
    return 0;
}

int safe_insert(Vektor *v, unsigned int index, ELEM e) {
    if (v->capacity > v->curr_size && index <= v->curr_size) {
        if (insert(v, index, e))
            return 1;
    }
    else if (v->capacity == v->curr_size && index <= v->curr_size) {
        if (set_capacity(v, (v->capacity) * 2)) {
            if (insert(v, index, e))
                return 1;
        }
    }
    return 0;
}