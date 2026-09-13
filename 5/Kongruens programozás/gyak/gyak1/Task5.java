class PrintTask extends Thread {
    private String text;

    public PrintTask(String text) {
        this.text = text;
    }

    @Override
    public void run() {
        // A terminálási feltétel 10-re módosítva
        for (int i = 0; i < 10; i++) {
            System.out.println(text + " - " + i);
            
            try {
                // 5 ezredmásodperc várakozás két kiírás között
                Thread.sleep(5);
            } catch (InterruptedException e) {
                // Ha a szálat megszakítják (interrupt), ide fut be a program
                System.out.println("\n[Rendszerüzenet] A(z) " + text + " szál megszakítva.");
                return; // Kilépés a run() metódusból, amivel a szál leáll
            }
        }
    }
}

public class Task5 {
    public static void main(String[] args) {
        // Szálak létrehozása
        PrintTask thread1 = new PrintTask("Hello");
        PrintTask thread2 = new PrintTask("World");

        // Szálak elindítása
        thread1.start();
        thread2.start();

        try {
            // A fő szál (main) várakozik 1 másodpercet (1000 ezredmásodperc)
            Thread.sleep(1000);

            // 1 másodperc után megszakítjuk a szálak futását
            thread1.interrupt();
            thread2.interrupt();

            // Bevárjuk, amíg a szálak ténylegesen leállnak
            thread1.join();
            thread2.join();
            
        } catch (InterruptedException e) {
            System.out.println("A fő program futása megszakadt.");
        }

        // Miután mindkét szál végzett, kiírjuk az eredményt
        System.out.println("Kész.");

        // --- Újraindítási kísérlet ---
        System.out.println("\nMegpróbáljuk újraindítani az egyik szálat...");
        try {
            thread1.start();
        } catch (IllegalThreadStateException e) {
            // Ide fog futni a program, mert a szálat nem lehet újraindítani
            System.out.println("Hiba: Egy már leállt vagy megszakított szálat nem lehet újraindítani.");
            System.out.println("A hiba típusa: IllegalThreadStateException");
        }
    }
}