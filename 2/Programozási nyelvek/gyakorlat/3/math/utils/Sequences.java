package math.utils;

public class Sequences {
    public int Fibonacci(int n) {
        if (n == 0) {
            return 0;
        } else if (n == 1) {
            return 1;
        } else {
            int result = 0;
            int prev = 0;
            int curr = 1;
            for (int i = 2; i <= n; i++) {
                result = prev + curr;
                prev = curr;
                curr = result; 
            }
            return result;
        }
    }

    public long Catalan(int n) {
        MathHelper mathHelper = new MathHelper();
        return mathHelper.Factorial(2 * n) / (mathHelper.Factorial(n + 1) * mathHelper.Factorial(n));
    }
}
