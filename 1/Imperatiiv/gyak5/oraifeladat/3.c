#include <stdio.h>

int main()
{
    int a = 0;
    int arr[7] = {0, 2, 8, 9, 30, 20, 50};
    for (int i = 0; i < 7; ++i)
    {
        if (arr[i] < arr[i + 1])
        {
            a = arr[i + 1];
        }
    }
    printf("%d\n", a);
    return 0;
}