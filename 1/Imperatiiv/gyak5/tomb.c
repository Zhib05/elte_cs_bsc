#include <stdio.h>

int main()
{
    int arr[2] = {1, 2};
    printf("%d %d\n", arr[0], arr[1]);

    int arr2[] = {1, 2, 3, 4};
    printf("%d sz=%zu\n", arr2[3], sizeof(arr2));

    int arr3[3] = {1};
    printf("%d %d %d\n", arr3[0], arr3[1], arr3[2]);

    for (int i = 0; i < 4; ++i)
    {
        printf("arr2[%d] = %d\n", i, arr2[i]);
    }

    // for (int i = 0; i < sizeof(arr)/sizeof(arr[0]))
    return 0;
}