#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char const *argv[])
{
    if (argc != 2)
    {
        fprintf(stderr, "hiba\n");
        return 1;
    }

    FILE* fp = fopen(argv[1], "r");

    if (fp == NULL)
    {
        return 1;
    }

    char buf[1023];
    int countsor = 0;
    while(fgets(buf, sizeof(buf), fp) != NULL)
    {
        buf[strlen(buf) - 1] = '\0';
        // if (strcmp(buf, "alma") == 0)
        // {
        //     ++countsor;
        // }

        if (strcmp(buf, "alma\n") == 0)
        {
            ++countsor;
        }
    }

    printf("%d\n", countsor);

    fclose(fp);
    return 0;
}