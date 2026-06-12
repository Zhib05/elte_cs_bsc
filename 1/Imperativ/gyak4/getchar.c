#include <stdio.h>
#include <ctype.h>

char to_lower(char ch) {
    if ('A' <= ch && ch <= 'Z') {
        return ch + ('a' - 'A');
    }
    return ch;

    // switch (ch) {
    //     case 'a' : return 'A';
    //     case 'b' : return 'B';
    // }
}

char to_upper(char x) {
    if ('A' <= x && x <= 'Z') {
        return 'a' + (x - 'A');
    }
    return x;
}

int main() {
    int input;
    /*EOF == End Of File*/
    while ((input = getchar()) != EOF) {
        // printf("%c", input);
        if (isupper(input)) {
            printf("%c", tolower(input));
        }
        else if (islower(input)) {
            printf("%c", toupper(input));
        }
        else {
            printf("%c", input);
        }
    }
}