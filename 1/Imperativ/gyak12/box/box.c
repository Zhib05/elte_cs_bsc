#include "box.h"
#include <stddef.h>
#include <stdlib.h>
#include <stdio.h>

static struct Box *top;

void initialize() {
    top = NULL;
}

int is_empty() {
    return top == NULL;
}

int peek() {
    if (top == NULL) {
        fprintf(stderr, "Tried to peek empty stack\n");
        abort();
    }
    return top->weight;
}

void push(int weight) {
    struct Box *newTop = malloc(sizeof(struct Box));

    newTop->weight = weight;
    newTop->next = top;

    top = newTop;
}

void pop() {
    if (top == NULL) {
        fprintf(stderr, "Tried to peek empty stack\n");
        abort();
    }

    struct Box* old_top = top;
    top = top->next;
    free(old_top);
}

void copy_top() {
    if (is_empty())
    return;

    struct Box* new_top = malloc(sizeof(struct Box));
    new_top->weight = peek();
    new_top->next = top;
    top = new_top;
}