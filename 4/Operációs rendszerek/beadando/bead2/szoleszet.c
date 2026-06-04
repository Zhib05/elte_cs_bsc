#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/wait.h>
#include <signal.h>

void jelzes_kezelo(int signumber) {
    if (signumber == SIGUSR1) {
        printf("--- Egy biztos jelezte, hogy keszenall. ---\n");
    } else if (signumber == SIGUSR2) {
        printf("--- Egy biztos jelezte, hogy befejezte amunkat. ---\n");
    }
}

void tavaszi_munkak() {
    int pipe_megsemisit[2];
    int pipe_vedekezes[2];

    if (pipe(pipe_megsemisit) == -1 || pipe(pipe_vedekezes) == -1) {
        printf("Hiba a csovek letrehozasakor!\n");
        return;
    }

    signal(SIGUSR1, jelzes_kezelo);
    signal(SIGUSR2, jelzes_kezelo);

    // Megsemmisítés
    pid_t child1 = fork();
    if (child1 == 0) {
        close(pipe_megsemisit[1]);
        close(pipe_vedekezes[0]);
        close(pipe_vedekezes[1]);

        sleep(1); 
        kill(getppid(), SIGUSR1);

        char adat[300];
        while (read(pipe_megsemisit[0], adat, sizeof(adat)) > 0) {
            char hely[100], tabla[100], fajta[100];
            int ter, pusz;
            sscanf(adat, "%[^,],%[^,],%[^,],%d,%d", hely, tabla, fajta, &ter, &pusz);
            
            printf("Tisztelt %s hegykozseg, %s, %s tabla fomernoke! Az orszagos hegybiro utasitasara azonnal kerem megsemmisiteni a tablat.\n", hely, tabla, fajta);
        }
        
        close(pipe_megsemisit[0]);
        
        sleep(1); 
        kill(getppid(), SIGUSR2);
        exit(0);
    }

    // Védekezés
    pid_t child2 = fork();
    if (child2 == 0) {
        close(pipe_vedekezes[1]);
        close(pipe_megsemisit[0]);
        close(pipe_megsemisit[1]);

        sleep(2); 
        kill(getppid(), SIGUSR1);

        char adat[300];
        while (read(pipe_vedekezes[0], adat, sizeof(adat)) > 0) {
            char hely[100], tabla[100], fajta[100];
            int ter, pusz;
            sscanf(adat, "%[^,],%[^,],%[^,],%d,%d", hely, tabla, fajta, &ter, &pusz);
            
            printf("Tisztelt %s, %s tabla fomernoke! Indul a tavaszi nagy orszagos vedekezes, ne inditsanak permetezest.\n", hely, tabla);
        }
        
        close(pipe_vedekezes[0]);
        
        sleep(2); 
        kill(getppid(), SIGUSR2);
        exit(0);
    }

    // Szülő folyamat
    close(pipe_megsemisit[0]);
    close(pipe_vedekezes[0]);

    pause(); 
    pause(); 

    FILE *f = fopen("adatok.txt", "r");
    if (f) {
        char row[300];
        while (fgets(row, sizeof(row), f)) {
            row[strcspn(row, "\n")] = 0; 
            char h[100], t[100], faj[100];
            int ter, pusz;
            sscanf(row, "%[^,],%[^,],%[^,],%d,%d", h, t, faj, &ter, &pusz);

            if (pusz >= 30) {
                write(pipe_megsemisit[1], row, sizeof(row));
            } else {
                write(pipe_vedekezes[1], row, sizeof(row));
            }
        }
        fclose(f);
    }

    close(pipe_megsemisit[1]);
    close(pipe_vedekezes[1]);

    pause();
    pause();

    wait(NULL);
    wait(NULL);

    printf("\nJelentes a miniszternek: A munkalatok befejezodtek.\n");

    f = fopen("adatok.txt", "r");
    FILE *temp = fopen("temp.txt", "w");
    if (f != NULL && temp != NULL) {
        char row[300];
        int volt_torles = 0;
        
        while (fgets(row, sizeof(row), f)) {
            char h[100], t[100], faj[100];
            int ter, pusz;
            sscanf(row, "%[^,],%[^,],%[^,],%d,%d", h, t, faj, &ter, &pusz);
            
            if (pusz < 30) {
                fprintf(temp, "%s", row);
            } else {
                volt_torles = 1; 
            }
        }
        fclose(f);
        fclose(temp);
        
        if (volt_torles) {
            remove("adatok.txt");
            rename("temp.txt", "adatok.txt");
        } else {
            remove("temp.txt"); 
        }
    }
}

void listazas(int tipus) {
    FILE *f = fopen("adatok.txt", "r");
    if (!f) return;

    char keresett[100] = "";
    if (tipus == 3) { printf("Melyik termohelyet keresi? "); scanf(" %[^\n]", keresett); }
    if (tipus == 4) { printf("Melyik fajtat keresi? "); scanf(" %[^\n]", keresett); }

    char row[300], t_hely[100], t_tabla[100], t_fajta[100];
    int t_terulet, t_pusztulas;

    printf("\n--- LISTA ---\n");
    while (fgets(row, sizeof(row), f)) {
        sscanf(row, "%[^,],%[^,],%[^,],%d,%d", t_hely, t_tabla, t_fajta, &t_terulet, &t_pusztulas);

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
        printf("\n1. Adatfelvetel\n2. Teljes lista\n3. Lista termohely szerint\n4. Lista fajta szerint\n5. Adat torlese/modositasa\n6. Tavaszi munkak vegrehajtasa\n0. Kilepes\nValasztas: ");
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
            char keresett_tabla[100], row[300], t_hely[100], t_tabla[100], t_fajta[100];
            int t_ter, t_pusz, mod_vagy_torol;

            printf("Adja meg a TABLA nevet, amit modositana/torolne: ");
            scanf(" %[^\n]", keresett_tabla);
            printf("1. Modositas, 2. Torles: ");
            scanf("%d", &mod_vagy_torol);

            while (fgets(row, sizeof(row), f)) {
                sscanf(row, "%[^,],%[^,],%[^,],%d,%d", t_hely, t_tabla, t_fajta, &t_ter, &t_pusz);
                if (strcmp(t_tabla, keresett_tabla) == 0) {
                    if (mod_vagy_torol == 1) {
                        printf("Uj termohely: "); scanf(" %[^\n]", t_hely);
                        printf("Uj fajta: "); scanf(" %[^\n]", t_fajta);
                        printf("Uj terulet: "); scanf("%d", &t_ter);
                        printf("Uj pusztulas mertek (%%): "); scanf("%d", &t_pusz);
                        fprintf(t, "%s,%s,%s,%d,%d\n", t_hely, t_tabla, t_fajta, t_ter, t_pusz);
                    }
                } else {
                    fprintf(t, "%s", row);
                }
            }
            fclose(f); fclose(t);
            remove("adatok.txt");
            rename("temp.txt", "adatok.txt");
            printf("Sikeres vegrehajtas.\n");
        }
        else if (valasztas == 6) {
            tavaszi_munkak();
            valasztas = 0;
        }
    } while (valasztas != 0);

    return 0;
}