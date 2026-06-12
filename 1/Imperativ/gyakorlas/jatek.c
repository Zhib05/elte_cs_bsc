#include <stdio.h>
#include <time.h>
#include <stdlib.h>

#define LIMIT 100
#define Max(a,b) ((a) > (b) ? (a) : (b))

int get_target()
{
    return rand() % LIMIT;
}

int read_num()
{
    printf("Adj meg egy szamot 0 es %d kozott: ", LIMIT - 1);
    int num;
    scanf("%d", &num);
    return num;
}

int main()
{
    srand(time(NULL));
    int szam = get_target();
    int tipp = read_num();

    while (tipp != szam)
    {
        printf("%s", tipp > szam ? "Tul nagy\n" : "Tul kicsi\n");
        tipp = read_num();
    }
    printf("gratulalok eltalaltad\n");
    // int a = rand() % 100+1;
    
    // int b = 0;
    // printf("irj be egy szamot 1-tol 99-ig: ");
    // scanf("%d", &b);

    // int i;
    // for (i = 1; b != a; i++) {
    //     if (b < a) {
    //         printf("tul kicsi: ");
    //         scanf("%d", &b);
    //     }
    //     else if (b > a) {
    //         printf("tul nagy: ");
    //         scanf("%d", &b);
    //     }
    //     else {
    //         printf("eltalaltad\n");
    //         break;
    //     }
    // }
    // printf("%d\n probalkozasbol talaltadki", i);
    return 0;
}