#include "exam.h"
#include <stdlib.h>
#include <string.h>

struct Exam* create_exam(char *sub, int dur, enum Type type) 
{
    struct Exam *exam = malloc(sizeof(struct Exam));
    exam->duration = dur;
    exam->type = type;
    strcpy(exam -> subject, sub);
    return exam;
}