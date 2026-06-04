#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define stud_cnt 5

void init_students(int *id, double *avg, short *age, int count)
{
    for (int i = 0; i < count; ++i)
    {
        id[i] = rand();
        avg[i] = 1.0 + (rand() % 41) / 10.0;
        age[i] = 16 + (rand() % 85);
    }
}

int main()
{
    srand(time(NULL));

    int id[stud_cnt];
    double avg[stud_cnt];
    short age[stud_cnt];

    init_students(id, avg, age, stud_cnt);

    for (int i = 0; i < 5; ++i)
    {
        printf("{ (%d : %f) }\n", id[i], avg[i]);
    }

    return 0;
}