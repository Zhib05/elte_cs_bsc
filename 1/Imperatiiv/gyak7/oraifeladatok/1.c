#include <stdio.h>
#include <stdlib.h>

int from_hex(char ch)
{
    if (ch >= '0' && ch <= '9')
    {
        return ch - '0';
    }
    if (ch >= 'a' && ch <= 'f')
    {
        return ch - 'a' + 10;
    }
//     printf("Not a valid Hex character\n");
//     exit(-1);
    return -1;
}

int main()
{
    long long value = 0;

    int ch;
    while ((ch = getchar()) != EOF)
    {
        int val = from_hex(ch);
        if (val == -1)
        {
            printf("hibas kaarakter!\n");
            continue;
        }
        value = value * 16 + val;

        printf("\n%lld\n", value);
    }
}