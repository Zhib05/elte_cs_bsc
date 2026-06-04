package arraydemo;

public class ArrayDemo {
    public static int max1(int[] numbers) {
        if (numbers.length == 0) {
            return 0;
        }

        int max = Integer.MIN_VALUE;

        for (int i = 0; i < numbers.length; ++i) {
            if (numbers[i] > max) {
                max = numbers[i];
            } else {
                max = max;
            }
        }
        return max;
    }

    public static int max2(int[] numbers) {
        if (numbers.length == 0) {
            return 0;
        }

        int max = Integer.MIN_VALUE;

        for (int i = 0; i < numbers.length; ++i) {
            max = numbers[i] > max ? numbers[i] : max;
        }
        return max;
    }

    public static int max3(int[] numbers) {
        if (numbers.length == 0) {
            return 0;
        }

        int max = Integer.MIN_VALUE;

        for (int i = 0; i < numbers.length; ++i) {
            max = Math.max(numbers[i], max);
        }
        return max;
    }

    public static int max4(int[] numbers) {
        if (numbers.length == 0) { return 0; }
        int max = Integer.MIN_VALUE;

        for (int currentNumber : numbers) {
            max = Math.max(currentNumber, max);
        }

        return max;
    }
}