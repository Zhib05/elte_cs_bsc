import java.io.*;

class WorldThread extends Thread {
    // 1. Itt tároljuk a kapott writer-t
    private PrintWriter writer;

    // Konstruktor: ezen keresztül kapja meg a szál a writer-t a főprogramból
    public WorldThread(PrintWriter writer) {
        this.writer = writer;
    }

    @Override
    public void run() {
        for (int i = 0; i < 10; ++i) {
            writer.print("World ");
        }
    }
}

class HelloThread extends Thread {
    private PrintWriter writer;

    public HelloThread(PrintWriter writer) {
        this.writer = writer;
    }

    @Override
    public void run() {
        for (int i = 0; i < 10; ++i) {
            writer.print("Hello ");
        }
    }
}

public class Main {
    public static void main(String[] args) {
        // 2. Hibakezelés (try-catch blokk) a fájlműveletek miatt
        try {
            PrintWriter writer = new PrintWriter(new FileWriter("output.txt"));
            
            // Átadjuk a writer-t a szálaknak
            Thread worldThread = new WorldThread(writer);
            Thread helloThread = new HelloThread(writer);
            
            worldThread.start();
            helloThread.start();
            
            // 4. Megvárjuk, amíg mindkét szál befejezi a munkát
            worldThread.join();
            helloThread.join();
            
            // 3. Lezárjuk a fájlt, hogy a tartalom elmentődjön
            writer.close();
            
            System.out.println("A kiírás sikeresen befejeződött!");
            
        } catch (IOException | InterruptedException e) {
            // Ha bármilyen hiba történik (pl. nem lehet létrehozni a fájlt), kiírjuk
            System.out.println("Hiba történt: " + e.getMessage());
        }
    }
}