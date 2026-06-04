package enumdemo;

public enum WildAnimal {
    MONKEY("Banana", 20),
    TIGER("Monkey", 4),
    ELEPHANT("Alma", 100),
    SNAKE("Elephant", 0.5);

    public String favoriteFood;
    public double amount; // napi preferált kaja bevitel

    WildAnimal(String favoriteFood, double amount) {
        this.favoriteFood = favoriteFood;
        this.amount = amount;
    }

    public static String listAllAnimals() {
        StringBuilder str = new StringBuilder();
        for (WildAnimal currentAnimal : WildAnimal.values()) {
            str.append(currentAnimal.ordinal() + ": ");
            str.append(currentAnimal.name() + " likes to eat ");
            str.append(currentAnimal.favoriteFood + " at least ");
            str.append(currentAnimal.amount + " times a day.\n");
        }
        return str.toString();
    }
}