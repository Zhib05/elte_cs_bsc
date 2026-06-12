#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char *read_str() {
    char buf[21];
    char *ptr = fgets(buf, 21, stdin);

    if (ptr == NULL) {
        return ptr;
    }

    char *copy = malloc(strlen(buf) + 1);
    strcpy(copy, buf);
    return copy;
}

int main() {
    char *copy = read_ptr();
    free(copy);
    return 0;
}
