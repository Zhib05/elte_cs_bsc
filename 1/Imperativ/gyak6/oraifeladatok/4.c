#include <stdio.h>

int sum(int *p, int count)
{
    int res = 0;
    int* end = p + count;
    // while (++p < end)
    for (; p < end; ++p)
    {
        res += *p;
    }
    return res;
}