import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class Task6 {

    // A szálak logikáját tartalmazó osztály
    static class FileWorker extends Thread {
        public FileWorker(String name) {
            super(name); // Beállítjuk a szál nevét
        }

        @Override
        public void run() {
            for (int i = 1; i <= 10000; i++) {
                // Fájl megnyitása, írása, majd automatikus bezárása a try-with-resources segítségével
                try (PrintWriter writer = new PrintWriter(new FileWriter(getName() + ".txt"))) {
                    writer.print(i);
                } catch (IOException e) {
                    // A feladat szerint elegendő az üres catch ág
                }
            }
        }
    }

    public static void main(String[] args) {
        List<FileWorker> threads = new ArrayList<>();

        System.out.println("Szálak indítása...");

        // 10 szál létrehozása és elindítása
        for (int i = 1; i <= 10; i++) {
            FileWorker worker = new FileWorker("Szal-" + i);
            threads.add(worker);
            worker.start();
        }

        // 1 másodperc várakozás
        try {
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            // Üres catch ág
        }

        System.out.println("Szálak bevárása (join)... ez eltarthat egy kis ideig a fájlműveletek miatt.");

        // A szálak biztonságos bevárása olvasás előtt (A módosított megoldás)
        for (FileWorker worker : threads) {
            try {
                worker.join();
            } catch (InterruptedException e) {
                // Üres catch ág
            }
        }

        // Fájlok tartalmának beolvasása és kiírása
        System.out.println("\nFájlok tartalma a szálak leállása után:");
        for (FileWorker worker : threads) {
            try (BufferedReader reader = new BufferedReader(new FileReader(worker.getName() + ".txt"))) {
                String utolsoSor = reader.readLine();
                System.out.println(worker.getName() + " utolsó száma: " + utolsoSor);
            } catch (IOException e) {
                // Üres catch ág
            }
        }
    }
}