public class Main {
    public static void main(String[] args) {
        Thread worldThread = new Thread(() -> {
            for (int i = 0; i < 10; ++i) {
                System.out.print("World ");
            }
        });
        Thread helloThread = new HelloThread();
        worldThread.start();
        helloThread.start();
    }
}
class HelloThread extends Thread {
    @Override
    public void run() {
        for (int i = 0; i < 10; ++i) {
            System.out.print("Hello ");
        }
    }
}