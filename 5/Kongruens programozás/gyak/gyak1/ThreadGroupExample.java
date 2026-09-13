public class ThreadGroupExample {
    public static void main(String[] args) {
        // 1. ThreadGroup (szálcsoport) létrehozása
        ThreadGroup myGroup = new ThreadGroup("Feldolgozó Csoport");

        // Egy egyszerű feladat, amely 2 másodpercig fut (hogy legyen időnk megfigyelni)
        Runnable task = () -> {
            try {
                Thread.sleep(2000);
            } catch (InterruptedException e) {
                System.out.println(Thread.currentThread().getName() + " megszakadt.");
            }
        };

        // 2. Szálak létrehozása, csoporthoz rendelése és elnevezése
        // A Thread konstruktora: Thread(szálcsoport, feladat)
        Thread thread1 = new Thread(myGroup, task);
        thread1.setName("Feldolgozó-1");

        Thread thread2 = new Thread(myGroup, task);
        thread2.setName("Feldolgozó-2");

        // Szálak elindítása
        thread1.start();
        thread2.start();

        // Rövid szünet a főprogramban, hogy a szálak biztosan elinduljanak a vizsgálat előtt
        try {
            Thread.sleep(500);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        // 3. Megfigyelés az activeCount() és list() metódusokkal
        System.out.println("--- Szálak állapota futás közben ---");
        
        // Visszaadja a csoportban lévő aktív (futó) szálak számát
        int active = myGroup.activeCount();
        System.out.println("Aktív szálak száma: " + active);
        
        System.out.println("\nA szálcsoport részletes felépítése (list metódus kimenete):");
        // A list() a rendszer szabványos kimenetére (konzolra) írja a csoport adatait
        myGroup.list();

        // Megvárjuk, amíg mindkét szál befejezi a munkát
        try {
            thread1.join();
            thread2.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        // Ellenőrzés a leállás után
        System.out.println("\n--- Szálak állapota a befejezés után ---");
        System.out.println("Aktív szálak száma: " + myGroup.activeCount());
    }
}