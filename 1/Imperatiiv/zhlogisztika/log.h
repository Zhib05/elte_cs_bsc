#ifndef LOG_H
#define LOG_H

typedef struct Node {
    int elem;
    struct Node *left;
    struct Node *right;
} Node;

Node* insert_pkg(Node *root, int distance);
void print_tree(Node *root, int depth);

#endif