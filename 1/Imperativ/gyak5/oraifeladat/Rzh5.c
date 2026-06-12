#include <stdio.h>

int main()
{
    int a[3] = {1,2,3};
    int b[3] = {1,2,3};
    int x = 0;

    for (int i = 0; i < 3; i++)
    {
        x = x + (a[i] * b[i]);
    }
    
    for (int i = 0; i < 3; ++i)
    {
        b[i] = b[i] * x;
        printf("%d ", b[i]);
    }
    
    return 0;
}