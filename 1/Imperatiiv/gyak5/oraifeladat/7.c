#include <stdio.h>

int compare(char *a, char *b)
{
    int i = 0;
    while (a[i] != '\0' && b[i] != '\0')
    {
        if (a[i] < b[i])
        {
            return -1;
        }
        if (a[i] > b[i])
        {
            return 1;
        }
        i++;
    }

    if (a[i] == '\0' && b[i] == '\0')
    {
        return 0;
    }
    if (a[i] == '\0')
    {
        return -1;
    }
    return 1;
}

int main()
{


    // char arr[100];
    // fgets(arr, sizeof(arr), stdin);

    // char arr2[100];
    // fgets(arr2, sizeof(arr2), stdin);

    // for (int i = 0; i < sizeof(arr);)
    // {
    //     if(arr[i] == arr2[i])
    //     {
    //         ++i;
    //     }
    //     else if(arr[i] < arr2[i])
    //     {
    //         printf("Az arr van elorebb\n");
    //         break;
    //     }
    //     else if(arr2[i] < arr[i])
    //     {
    //         printf("Az arr2 van elorebb\n");
    //         break;
    //     }
        
    // }

    return 0;
}