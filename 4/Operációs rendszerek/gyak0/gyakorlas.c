#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h> //open,creat
#include <sys/types.h> //open
#include <sys/stat.h>
#include <errno.h> //perror, errno
#include <unistd.h>
#include <time.h>

int main(int argc, char** argv) {
    if (argc != 2) {
        perror("You have to use program with one arguments");
        exit(1);
    }
    int f;

    f = open(argv[1], O_WRONLY | O_CREAT | O_TRUNC, S_IRUSR | S_IWUSR);

    if (f < 0) {
        perror("Error at opening the file\n");
        exit(1);
    }

    srand(time(NULL));
    for (int i = 0; i < 10; i++)
    {
        int r = rand() % 100;

        printf("Writing: %i\n", r);
        if (write(f, &r, sizeof(int)) != sizeof(int))
        {
            perror("There is a mistake in writing\n");
            exit(1);
        }
    }
    close(f);

    f = open(argv[1], O_RDONLY);

    for (int i = 0; i < 10; i++)
    {
        int x;
        if (read(f, &x, sizeof(int)) != sizeof(int))
        {
            perror("There is a mistake in reading\n");
            exit(1);
        }
        printf("Reading: %i\n", x);
    }

    close(f);
    return 0;
}