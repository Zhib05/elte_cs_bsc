#include <stdio.h>
#include <stdlib.h>

int main() {
    int tmp;
    int cnt = 0;
    int* arr = NULL;

    while (scanf("%d", &tmp) != EOF) {
        arr = realloc(arr, sizeof(int) * cnt++);
        arr[cnt - 1] = tmp;
    }

    puts("A beolvasott szamok: ");
    for(int i = cnt - 1; i >= 0; --i) {
        printf("%d", arr[i]);
    }
    puts(" ");

    return 0;
}