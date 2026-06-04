#include <stdio.h>

int main()
{
    int a = 0;
    int arr[5] = {1, 2, 3, 4};
    for (int i = 0; i < sizeof(arr)/sizeof(arr[0]); ++i)
    {
        a = a + arr[i];
    }
    printf("%d", a);
    return 0;
}