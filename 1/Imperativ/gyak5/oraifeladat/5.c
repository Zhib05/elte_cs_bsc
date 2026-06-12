#include <stdio.h>

int main()
{
    int a = 0;
    int arr[5] = {1, 2, 3, 4};
    int arr2[5] = {1, 2, 3, 4};
    for (int i = 0; i < 5; ++i)
    {
        a = a + arr[i] * arr2[i];
    }
    printf("%d", a);
    return 0;
}