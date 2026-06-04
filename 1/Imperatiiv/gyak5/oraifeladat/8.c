#include <stdio.h>
#include <string.h>

int main()
{
    char arr[100];

    printf("Irja be egy stringet: ");
    fgets(arr, sizeof(arr), stdin);

    int length = strlen(arr);
    printf("A beirt string hossza enterrel egyut: %d\n", length);
    if (arr[(length - 1)] == '\n')
    {
        arr[(length - 1)] = '\0';
        length--;
    }

    printf("A beirt string hossza: %d\n", length);

    return 0;
}