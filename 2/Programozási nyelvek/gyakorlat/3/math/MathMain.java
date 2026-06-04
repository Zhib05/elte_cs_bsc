package math;

// import math.utils.MathHelper;
import math.utils.Sequences;

public class MathMain {
    public static void main(String[] args) {
        // MathHelper mathHelper = new MathHelper();
        Sequences sequences = new Sequences();
        
        System.out.print("Fibonacci: ");
        for (int i = 0; i < 10; i++) {
            System.out.print(sequences.Fibonacci(i) + " ");
        }
        System.out.println();

        System.out.print("Catalang: ");
        for (int i = 0; i < 10; i++) {
            System.out.print(sequences.Catalan(i) + " ");
        }
        System.out.println();
    }
}
