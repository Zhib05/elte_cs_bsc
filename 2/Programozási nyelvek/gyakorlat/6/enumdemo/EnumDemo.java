package enumdemo;

public class EnumDemo {
    public static void main(String[] args) {
        TrafficLigth ligth = new TrafficLigth();
        ligth.signal();

        System.out.println(WildAnimal.ELEPHANT.favoriteFood);

        System.out.println(WildAnimal.listAllAnimals());
    }
}