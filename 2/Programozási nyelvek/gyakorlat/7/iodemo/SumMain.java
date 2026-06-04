package iodemo;

public class SumMain {
    public static void main(String[] args) {
        String fileName = "iodemo/in.txt";
        try {
            System.out.println(Sum.addNumbers(fileName));
        } catch (NullPointerException e) {
            System.out.println(Sum.addNumbers("iodemo/defaultIn.txt"));
        }
    }
}