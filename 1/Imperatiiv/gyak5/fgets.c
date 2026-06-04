#include <stdio.h>
#include <string.h>

int main()
{
    FILE *fp = fopen("test.txt", "r");
    char file[1024];
    while(fgets(file, 1023, fp) != NULL) {
        // file[1023] = '\0';
        file[strcspn(file, "\r\n")] = 0;
        printf("%s\n", file);
    }
    return 0;

    /*
        >> 2 => osztas 4-el
        >> k => osztas 2 a k-adikonnal
        << 2 => szorzas 4-el
    */
}