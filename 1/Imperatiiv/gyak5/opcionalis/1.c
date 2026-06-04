#include <stdio.h>

int main() 
{
    int arr[] = {4, 5, 1, -1, 9};
    int size = sizeof(arr) / sizeof(arr[0]);
    int minIndex = 0, maxIndex = 0;

    for (int i = 1; i < size; i++)
    {
        if (arr[i] < arr[minIndex])
        {
            minIndex = i;
        }
        if (arr[i] > arr[maxIndex])
        {
            maxIndex = i;
        }
    }

    int temp = arr[minIndex];
    arr[minIndex] = arr[maxIndex];
    arr[maxIndex] = temp;

    printf("csere után: ");
    for (int i = 0; i < size; i++)
    {
        printf("%d ", arr[i]);
    }
    return 0;
}