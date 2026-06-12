#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define stud_cnt 5

typedef struct student
{
    int id;
    double avg;
    short age;
    struct student *p;
} Student;


void init_students(Student *students, int count)
{
    for (int i = 0; i < count; ++i)
    {
        students[i].id = rand();
        students[i].avg = 1.0 + (rand() % 41) / 10.0;
        students[i].age = 16 + (rand() % 85);
    }
}

int main()
{
    Student students[stud_cnt];

    srand(time(NULL));

    init_students(students, stud_cnt);

    for (int i = 0; i < 5; ++i)
    {
        printf("{ (%d : %f : %u) }\n", students[i].id, students[i].avg, students[i].age);
    }

    return 0;
}