#include <stdio.h>

int Sum (int x)
{
    int i, y;
    for (i = 1; i <= x; ++i)
    {
        y += i;
    }
    return y;
}

int main()
{
    int n;
    scanf("%d", &n);
    printf("%d\n", Sum(n));
    return 0;
}