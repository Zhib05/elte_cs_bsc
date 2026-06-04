#include <stdio.h>
int main()
{
    int matrix[2] [2] = {
        {1, 2},
        {3, 4}
    };

    int matrix2[] [2] = {
        {0, 1},
        {1, 0}
    };

    printf ("%d\n", matrix[1][0]); //matrix kiirasa 0-dik sor 0-dik oszlop

    int matrix3 [2] [2] = {1, 2, 3, 4};
    printf ("%d\n", matrix3[1][0]);

    return 0;
}