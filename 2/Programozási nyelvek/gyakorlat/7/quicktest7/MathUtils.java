package quicktest7;

class MathUtils {
    public static int nextNaturalNumber(int number) {
        if (number < 0) {
            throw new IllegalArgumentException();
        }
        return number + 1;
    }
}