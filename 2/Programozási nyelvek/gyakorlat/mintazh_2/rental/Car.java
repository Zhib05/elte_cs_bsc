package rental;

public class Car {
    private String brand;
    private String licensePlate;
    private double price;
    private static final double MAX_PRICE = 500.0;
    private static final Car CAR_OF_THE_YEAR = new Car("Alfa Romeo", "ABC 123", MAX_PRICE);

    private Car(String brand, String licensePlate, double price) {
        this.brand = brand;
        this.licensePlate = licensePlate;
        this.price = price;
    }

    public double getPrice() {
        return price;
    }

    private static boolean isValidLicensePlate(String licensePlate) {
        if (!(licensePlate.length() == 7))
            return false;
        String[] plate = licensePlate.split(" ");
        if (!(plate[0].length() == 3 && plate[1].length() == 3))
            return false;
        for(int i = 0; i < 3; ++i) {
            if (!(Character.isUpperCase(plate[0].charAt(i)) && Character.isDigit(plate[1].charAt(i))))
                return false;
        }
        return true;
    }

    public static Car make(String brand, String licensePlate, double price) {
        if (brand == null || brand.length() < 2) {
            return null;
        }
        for (char c : brand.toCharArray()) {
            if (!Character.isLetter(c) && c != ' ') {
                return null;
            }
        }
        if (!isValidLicensePlate(licensePlate)) {
            return null;
        }
        if (price <= 0 || price > MAX_PRICE) {
            return null;
        }
        return new Car(brand, licensePlate, price);
    }

    public void decreasePrice() {
        if (price > 10 && price != MAX_PRICE) {
            price -= 10;
        }
    }

    public boolean isCheaperThan(Car other) {
        if (other == null) {
            return false;
        }
        return this.price < other.price;
    }

    @Override
    public String toString() {
        return String.format("%s (%s) %.1f EUR", brand, licensePlate, price);
    }
}