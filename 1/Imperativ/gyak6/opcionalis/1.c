#include <stdio.h>

int *max_element(int *begin, int *end)
{
    int *maxp = begin;
    while (begin < end)
    {
        if (*begin > *maxp)
        {
            maxp = begin;
        }
        ++begin;
    }
    return maxp;
}

int main()
{
    int x[4] = {1, 2, 5, 4};

    int *max = max_element(&x[0], &x[4]);

    printf("max = %p\tmax = %d", max, *max);
}