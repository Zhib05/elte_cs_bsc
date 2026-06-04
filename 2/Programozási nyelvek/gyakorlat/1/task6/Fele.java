package task6;

public class Fele {
    public static void main(String[] args) {
        int a = Integer.parseInt(args[0]);
        int b = Integer.parseInt(args[1]);

        for (int i = a + 1; i < b; i++) {
            System.out.println(i / 2.0);
        }
    }
}