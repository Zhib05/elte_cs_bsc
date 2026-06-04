#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/wait.h>
#include <signal.h>
#include <sys/ipc.h>
#include <sys/msg.h>
#include <sys/sem.h>
#include <sys/types.h>
#include <string.h>
#include <time.h>

#define MAX_SZOVEG 100

// Üzenetsor struktúrája
struct uzenet {
    long mtype;   
    int db;    
};

void jelzes_kezelo(int signumber) {
    if (signumber == SIGUSR1) {
        printf("Matula bacsi vette: Egy fiu keszen all a feladatra.\n");
    }
}

void szemafor_muvelet(int semid, int op) {
    struct sembuf muvelet;
    muvelet.sem_num = 0;
    muvelet.sem_op  = op; 
    muvelet.sem_flg = 0;
    if(semop(semid, &muvelet, 1) < 0) {
        perror("Szemafor hiba");
    }
}

void szemafor_torles(int semid) {
    semctl(semid, 0, IPC_RMID);
}

int main() {
    signal(SIGUSR1, jelzes_kezelo);

    int msg_id = msgget(IPC_PRIVATE, IPC_CREAT | 0666);
    if (msg_id == -1) { perror("Uzenetsor hiba"); return 1; }

    int pipe_btol_mnek[2];
    int pipe_ttol_mnek[2];
    int pipe_mtol_bnek[2];

    if (pipe(pipe_btol_mnek) == -1 || 
        pipe(pipe_ttol_mnek) == -1 || 
        pipe(pipe_mtol_bnek) == -1) {
        perror("Csovezetek hiba");
        return 1;
    }

    int sem_id = semget(IPC_PRIVATE, 1, IPC_CREAT | 0666);
    semctl(sem_id, 0, SETVAL, 1);

    printf("--- A Berek tabor felebredt. Indul a nap! ---\n\n");

    // Tutajos
    pid_t tutajos = fork();
    if (tutajos == 0) {
        close(pipe_btol_mnek[0]); close(pipe_btol_mnek[1]);
        close(pipe_mtol_bnek[0]); close(pipe_mtol_bnek[1]);
        close(pipe_ttol_mnek[0]); 

        sleep(1);
        kill(getppid(), SIGUSR1);

        struct uzenet kapott_feladat;
        msgrcv(msg_id, &kapott_feladat, sizeof(int), 2, 0); 
        printf("Tutajos (Gyula): Megkaptam a feladatot! %d halat kell fognom ebedre.\n", kapott_feladat.db);

        srand(time(NULL) ^ getpid()); 
        int fogott_halak = (rand() % 2) + 1; 

        char veszjelzes[MAX_SZOVEG] = {0}; 
        strcpy(veszjelzes, "A csuka megfogott stop, segitseg stop!");
        write(pipe_ttol_mnek[1], veszjelzes, MAX_SZOVEG);
        
        fogott_halak++; 
        
        write(pipe_ttol_mnek[1], &fogott_halak, sizeof(int));

        close(pipe_ttol_mnek[1]);
        exit(0);
    }

    // Butyok
    pid_t butyok = fork();
    if (butyok == 0) {
        close(pipe_ttol_mnek[0]); close(pipe_ttol_mnek[1]);
        close(pipe_btol_mnek[0]); 
        close(pipe_mtol_bnek[1]); 

        sleep(2);
        kill(getppid(), SIGUSR1);

        struct uzenet kapott_feladat;
        msgrcv(msg_id, &kapott_feladat, sizeof(int), 1, 0); 
        printf("Butyok (Bela): Megkaptam a feladatot! %d koteg fat kell gyujtenem.\n", kapott_feladat.db);

        char hiba_jelzes[MAX_SZOVEG] = {0};
        strcpy(hiba_jelzes, "Vizes a fa!");
        write(pipe_btol_mnek[1], hiba_jelzes, MAX_SZOVEG);

        char uj_utasitas[MAX_SZOVEG];
        read(pipe_mtol_bnek[0], uj_utasitas, sizeof(uj_utasitas));
        printf("Butyok (Bela) kapta az utasitast: \"%s\"\n", uj_utasitas);
        printf("Butyok (Bela): Indulok segiteni Tutajosnak kifesziteni a csukat!\n");

        close(pipe_btol_mnek[1]);
        close(pipe_mtol_bnek[0]);
        exit(0);
    }

    // Matula
    close(pipe_btol_mnek[1]);  
    close(pipe_ttol_mnek[1]); 
    close(pipe_mtol_bnek[0]);  

    pause(); 
    pause(); 

    struct uzenet uzenet;
    uzenet.mtype = 1; 
    uzenet.db = 2; 
    msgsnd(msg_id, &uzenet, sizeof(int), 0);

    uzenet.mtype = 2; 
    uzenet.db = 3; 
    msgsnd(msg_id, &uzenet, sizeof(int), 0);
    
    printf("Matula bacsi: A feladatok kikuldve az uzenetsoron.\n\n");

    char jelzes_b[MAX_SZOVEG];
    read(pipe_btol_mnek[0], jelzes_b, MAX_SZOVEG);
    printf("Matula bacsi: Uzenet erkezett Butyoktol -> \"%s\"\n", jelzes_b);

    char jelzes_t[MAX_SZOVEG];
    read(pipe_ttol_mnek[0], jelzes_t, MAX_SZOVEG);
    printf("Matula bacsi: Veszjelzes erkezett Tutajostol -> \"%s\"\n", jelzes_t);

    char valasz_b[MAX_SZOVEG] = {0};
    strcpy(valasz_b, "Hagyja Bela a vizes fat masnak, segitsen a horgasznak!");
    write(pipe_mtol_bnek[1], valasz_b, MAX_SZOVEG);

    int vegleges_hal_szam = 0;
    read(pipe_ttol_mnek[0], &vegleges_hal_szam, sizeof(int));

    wait(NULL);
    wait(NULL);

    printf("\nMatula bacsi: Miutan a csukanak nem sikerult Magat megenni, orulok, hogy epsegben elokerult.\n");

    szemafor_muvelet(sem_id, -1);
    
    int napok = 1;

    FILE *f_r = fopen("napi_fogas.txt", "r");
    if (f_r != NULL) {
        char row[200];
        while (fgets(row, sizeof(row), f_r)) {
            napok++;
        }
        fclose(f_r);
    }

    FILE *f_w = fopen("napi_fogas.txt", "a");
    if (f_w != NULL) {
        fprintf(f_w, "%d. nap fogott halak szama : %d\n", napok, vegleges_hal_szam);
        fclose(f_w);
        printf("A %d. napon %d db halt sikerült fogni.\n", napok, vegleges_hal_szam);
    }
    
    szemafor_muvelet(sem_id, 1); 

    close(pipe_btol_mnek[0]);
    close(pipe_ttol_mnek[0]);
    close(pipe_mtol_bnek[1]);

    msgctl(msg_id, IPC_RMID, NULL);
    szemafor_torles(sem_id);

    return 0;
}