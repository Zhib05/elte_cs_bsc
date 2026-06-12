#ifndef BOX_H
#define BOX_H

struct Box {
    int weight;
    struct Box *next;
};

struct Stack {
    struct Box *top
};

void initialize(void);
int is_empty(void);
int peek(void);
void push(int weight);
void pop(void);
void copy_top(void);

#endif