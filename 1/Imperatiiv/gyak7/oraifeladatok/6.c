#include <stdio.h>
#include <stdlib.h>
#include <time.h>

void feltolt(double arr[], int a)
{
    for (int i = 0; i < a; ++i)
    {
        arr[i] = (((double)rand() / RAND_MAX) * 100);
    }
}

int nagyobb_50nel(double arr[], int n)
{
    int db = 0;
    for (int i = 0; i < n; ++i)
    {
        if(arr[i] > 50)
        {
            db++;
        }
    }

    return db;
}

int main()
{
    int n;
    scanf("%d", &n);

    double arr[n];
    srand(time(NULL));

    feltolt(arr[n], n);

    int nagyobb_mint_50 = nagyobb_50nel(arr, n);

    return 0;
}