#include <stdio.h>

int main ()
{
    double a = 0;
    double arr[5] = {1.5, 2, 3, 4};
    double arr2[5] = {1.5, 2, 3, 4};
    for (int i = 0; i < 5; ++i)
    {
        a = a + (arr[i] * arr2[i]);
    }
    printf("%lf", a / 2.0);
    return 0;
    return 0;
}