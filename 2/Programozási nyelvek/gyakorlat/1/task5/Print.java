public class Print {
    public static void main(String[] args) {
        for(double i = 1; i <= 4; i++) {
            System.out.println(i / 2);
        }
        int i = Integer.parseInt(System.console().readLine());
        System.console().printf("Kiirtam %d szamot", i);
    }
}