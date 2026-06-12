#include <stdio.h>

int main()
{
    int a = 0;
    int arr[7] = {0, 2, 8, 9, (-1), 3, (-2)};

    int min, second;
    if (arr[0] < arr[1])
    {
        min = arr[0];
        second = arr [1];
    }
    else if (arr[0] > arr[1])
    {
        min = arr[1];
        second = arr[0];
    }

    for (int i = 2; i < 7; ++i)
    {
        //min <= second
        if (arr[i] <= min)
        {
            second = min;
            min = arr[i];
        }
        else if (arr[i] < second)
        {
            second = arr[i];
        }
    }
    printf("%d\n", second);
}