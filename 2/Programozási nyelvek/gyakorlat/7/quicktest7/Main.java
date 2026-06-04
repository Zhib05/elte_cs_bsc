package quicktest7;

public class Main {
    public static void main(String[] args) {
        try {
            int result = MathUtils.nextNaturalNumber(-1);
            System.out.println(result);
        } catch (IllegalArgumentException e) {
            System.out.println("Number must be non-negative");
        }
    }
}