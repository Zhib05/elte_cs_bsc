#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct {
    char name[20];
    int *age
} Person;

int main() {
    int *agep = malloc(sizeof(int));
    Person p = { .name = "Sanyi", agep };
    *p.age = 42;

    Person p2 = p;
    strcpy(p2.name, "Jani");
    *p2.age = 20;

    printf("%s %d eves\n", p.name, *p.age);
    printf("%s %d eves\n", p2.name, *p2.age);

    return 0;
}
