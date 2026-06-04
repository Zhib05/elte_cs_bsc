#include <stdio.h>

int main()
{
    int a = 20;
    int* ptr = &a;
    *ptr = 30;

    printf("%d\n", a);

    return 0;
}