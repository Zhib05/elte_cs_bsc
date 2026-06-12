#include <stdio.h>

void func()
{
    int a = 10;
    if (a > 5)
    {
        printf("%d\n", a + 1);
    }
    else if (a > 5)
    {
        printf("%d\n", a - 1);
    }
    else
    {
        printf("%d\n", a);
    }
}

int main()
{
    func();
    return 0;
}