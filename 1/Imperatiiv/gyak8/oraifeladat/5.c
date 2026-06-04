#include <stdio.h>

int main()
{
    int i = 1;
    int j = 2;
    {
        int i = 10;
        int j = 20;
    }

    {
        i = 10;
        int j = 20;
    }
    printf("%d %d\n", i, j);
    return 0;
}