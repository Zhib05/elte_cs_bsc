#define STUD_CNT 17

typedef enum {
    BSc,
    MSc,
    PhD,
} Type;

struct PhDData {
    double impact_factor;
    int erdos_number;
};

typedef struct Student {
    int id;
    double avg;
    short age;
    Type type;
    union {
        int courses;
        double credits;
        struct PhDData phd;
    };
} AdvStudent;

void print_student(AdvStudent *student) {
    printf("id: %d\tavg: %f\tage: %d\n", student->id, student->avg, student->age);
    switch (student->type) {
        case BSc:
            printf("course: %d\n", student->courses);
            break;
        case MSc:
            printf("credits: %f\n", student->credits);
            break;
        case PhD:
            printf("impact factor: %f\tErdos number: %d\n", student->phd.impact_factor);
            break;
    };
}