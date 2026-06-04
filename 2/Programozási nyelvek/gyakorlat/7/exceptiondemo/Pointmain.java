package exceptiondemo;

public class Pointmain {
    public static void main(String[] args) {
        Point point1 = new Point();
        point1.setX(3);
        point1.setY(2);
        System.out.println(point1.getX());
        System.out.println(point1.getY());

        try {
            point1.setX(-2);
        } catch (IllegalArgumentException exception) {
            System.out.println(exception.getMessage());
        }
        System.out.println(point1.getX());
        System.out.println(point1.getY());
    }
}