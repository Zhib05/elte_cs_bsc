#include <stdio.h>
#include <string.h>
#include <stdlib.h>

void listazas(int tipus) {
    FILE *f = fopen("adatok.txt", "r");
    if (!f) return;

    char keresett[100] = "";
    if (tipus == 3) { printf("Melyik termohelyet keresi? "); scanf(" %[^\n]", keresett); }
    if (tipus == 4) { printf("Melyik fajtat keresi? "); scanf(" %[^\n]", keresett); }

    char sor[300], t_hely[100], t_tabla[100], t_fajta[100];
    int t_terulet, t_pusztulas;

    printf("\n--- LISTA ---\n");
    while (fgets(sor, sizeof(sor), f)) {
        sscanf(sor, "%[^,],%[^,],%[^,],%d,%d", t_hely, t_tabla, t_fajta, &t_terulet, &t_pusztulas);

        if (tipus == 2 || (tipus == 3 && strcmp(t_hely, keresett) == 0) || (tipus == 4 && strcmp(t_fajta, keresett) == 0)) {
            printf("Termohely: %s | Tabla: %s | Fajta: %s | Terulet: %d | Pusztulas: %d%%\n", t_hely, t_tabla, t_fajta, t_terulet, t_pusztulas);
        }
    }
    fclose(f);
    printf("-------------\n");
}

int main() {
    int valasztas;
    do {
        printf("\n1. Adatfelvetel\n2. Teljes lista\n3. Lista termohely szerint\n4. Lista fajta szerint\n5. Adat torlese/modositasa\n0. Kilepes\nValasztas: ");
        if (scanf("%d", &valasztas) != 1) {
            while (getchar() != '\n'); 

            printf("Hiba: Ervenytelen bemenet. Kerem, a menupontok szamai kozul valasszon!\n");
            
            valasztas = -1;
            continue;
        }

        if (valasztas == 1) {
            FILE *f = fopen("adatok.txt", "a");
            char h[100], t[100], faj[100];
            int ter, pusz;
            printf("Termohely: "); 
            scanf(" %[^\n]", h);

            printf("Tabla: "); 
            scanf(" %[^\n]", t);

            printf("Fajta: "); 
            scanf(" %[^\n]", faj);

            printf("Terulet: "); 
            scanf("%d", &ter);

            printf("Pusztulas (%%): "); 
            scanf("%d", &pusz);

            fprintf(f, "%s,%s,%s,%d,%d\n", h, t, faj, ter, pusz);
            fclose(f);
        } 
        else if (valasztas >= 2 && valasztas <= 4) {
            listazas(valasztas);
        }
        else if (valasztas == 5) {
            FILE *f = fopen("adatok.txt", "r");
            FILE *t = fopen("temp.txt", "w");
            char keresett_tabla[100], sor[300], t_hely[100], t_tabla[100], t_fajta[100];
            int t_ter, t_pusz, mod_vagy_torol;

            printf("Adja meg a TABLA nevet, amit modositana/torolne: ");
            scanf(" %[^\n]", keresett_tabla);
            printf("1. Modositas, 2. Torles: ");
            scanf("%d", &mod_vagy_torol);

            while (fgets(sor, sizeof(sor), f)) {
                sscanf(sor, "%[^,],%[^,],%[^,],%d,%d", t_hely, t_tabla, t_fajta, &t_ter, &t_pusz);
                if (strcmp(t_tabla, keresett_tabla) == 0) {
                    if (mod_vagy_torol == 1) {
                        printf("Uj termohely: "); scanf(" %[^\n]", t_hely);
                        printf("Uj fajta: "); scanf(" %[^\n]", t_fajta);
                        printf("Uj terulet: "); scanf("%d", &t_ter);
                        printf("Uj pusztulas mertek (%%): "); scanf("%d", &t_pusz);
                        fprintf(t, "%s,%s,%s,%d,%d\n", t_hely, t_tabla, t_fajta, t_ter, t_pusz);
                    }
                } else {
                    fprintf(t, "%s", sor);
                }
            }
            fclose(f); fclose(t);
            remove("adatok.txt");
            rename("temp.txt", "adatok.txt");
            printf("Sikeres vegrehajtas.\n");
        }
    } while (valasztas != 0);

    return 0;
}