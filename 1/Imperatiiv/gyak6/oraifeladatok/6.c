#include <stdio.h>

double avg(int *begin, int *end)
{
    double sum = 0;
    int size = end - begin;
    while (begin < end)
    {
        sum += *begin;
        ++begin;
    }

    return sum/size;
}

int main()
{
    // int arr[5] = {1, 2, 3, 4, 5};
    // int *begin = arr;
    // int *end = 5;
    // double avg();
}