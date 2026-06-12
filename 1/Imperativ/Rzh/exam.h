#ifndef EXAM_H
#define EXAM_H

enum Type
{
    Oral,
    Written
};

typedef struct Exam
{
    char subject[21];
    int duration;
    enum Type type;
};

struct Exam *create_exam(char *sub, int dur, enum Tipus type);
#endif