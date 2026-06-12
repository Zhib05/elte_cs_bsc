#include "log.h"
#include <stdio.h>
#include <stdlib.h>

Node *creat_pkg(int distance){
    Node *new_node = (Node *)malloc(sizeof(Node));
    if (new_node == NULL) {
        printf("Memory allocation failed\n");
        exit(EXIT_FAILURE);
    }
    new_node->elem = distance;
    new_node->left = NULL;
    new_node->right = NULL;
    return new_node;
}

Node *insert_pkg(Node *root, int distance) {
    if (root == NULL) {
        return creat_pkg(distance);
    }
    if (root->elem == distance) {
        printf("HIBA: A %d tavolsag mar letezik\n", distance);
        return root;
    }

    if (distance < root->elem) {
        root->left = insert_pkg(root->left, distance);
    } else {
        root->right = insert_pkg(root->right, distance);
    }
    return root;
}

void print_tree(Node* root, int depth) {
    if (root == NULL) {
        return;
    }

    // Bal oldali rész feldolgozása rekurzívan
    print_tree(root->left, depth + 1);

    // Aktuális elem kiírása megfelelő mélység szerint
    for (int i = 0; i < depth; i++) {
        printf("    ");  // Négy szóköz minden szinthez
    }
    printf("%d\n", root->distance);

    // Jobb oldali rész feldolgozása rekurzívan
    print_tree(root->right, depth + 1);
}