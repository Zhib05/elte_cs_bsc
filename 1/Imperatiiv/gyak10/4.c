#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char *reverse(char *str) {
    size_t n = strlen(str);
    if (n == 0) {
        return str;
    }

    size_t i = 0, j = n - 1;
    while (i < j) {
        char tmp = str[i];
        str[i] = str[j];
        str[j] = tmp;
        ++i;
        --j;
    }
    return str;
}

int main() {
    return 0;
}