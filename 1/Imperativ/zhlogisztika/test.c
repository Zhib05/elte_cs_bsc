#include <stdio.h>
#include <stdlib.h>

// Bináris keresőfa csomópontjának definíciója
typedef struct Node {
    int distance;
    struct Node* left;
    struct Node* right;
} Node;

// Új csomópont létrehozása
Node* create_pkg(int distance) {
    Node* newNode = (Node*)malloc(sizeof(Node));
    if (!newNode) {
        printf("Memóriafoglalási hiba!\n");
        return NULL;
    }
    newNode->distance = distance;
    newNode->left = NULL;
    newNode->right = NULL;
    return newNode;
}

// Új raklap hozzáadása a fához
Node* insert_pkg(Node* root, int distance) {
    if (root == NULL) {
        return create_pkg(distance);
    }
    if (distance < root->distance) {
        root->left = insert_pkg(root->left, distance);  // Balra helyezés
    } else if (distance > root->distance) {
        root->right = insert_pkg(root->right, distance);  // Jobbra helyezés
    } else {
        printf("A %d távolságú raklap már létezik a fában.\n", distance);
    }
    return root;
}

// Bináris fa kiírása mélység szerint
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

int main() {
    Node* root = NULL;

    // Tesztadatok hozzáadása
    int distances[] = {5, 3, 4, 2, 7, 12, 8, 18, 6, 1};
    int n = sizeof(distances) / sizeof(distances[0]);

    for (int i = 0; i < n; i++) {
        root = insert_pkg(root, distances[i]);
    }

    // Fa kiíratása
    printf("A bináris keresőfa tartalma:\n");
    print_tree(root, 0);

    return 0;
}
