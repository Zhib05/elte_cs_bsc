#include <stdio.h>
#include <string.h>

void csere (char arr[])
{
    int hossz = strlen(arr);
    int temp;
    if (hossz < 3)
    {
        printf("legalabb 3 jegyu szamot irjon be\n");
    }
    else
    {
        temp = arr[0];
        arr[0] = arr[hossz - 1];
        arr[hossz - 1] = temp;
        printf("%s\n", arr);
    }

}

int main()
{
    char arr[100];
    scanf("%s", arr);
    csere(arr);
    return 0;
}