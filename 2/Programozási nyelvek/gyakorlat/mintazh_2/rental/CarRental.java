package rental;

import java.io.IOException;
import java.io.FileReader;
import java.io.BufferedReader;
import java.util.ArrayList;
import java.util.Random;

public class CarRental {
    private ArrayList<Car> cars;

    public CarRental(String filename) {
        cars = new ArrayList<>();
        try (BufferedReader br = new BufferedReader(new FileReader(filename))) {
            String line;
            while ((line = br.readLine()) != null) {
            try {
                    String[] parts = line.split("[:,]");
                    if (parts.length != 3) {
                        continue;
                    }
                    String brand = parts[0].trim();
                    String licensePlate = parts[1].trim();
                    double price = Double.parseDouble(parts[2].trim());
                    Car car = Car.make(brand, licensePlate, price);
                    if (car != null) {
                        cars.add(car);
                    }
                } catch (Exception e) {
                    continue;
                }
            } 
        } catch (IOException e) {
            cars = new ArrayList<>();
        }
    }

    public int numberOfCars() {
        return cars.size();
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        for (Car car : cars) {
            sb.append(car.toString()).append("\n");
        }
        return sb.toString();
    }

    public void insertionSort() {
        for (int i = 1; i < cars.size(); i++) {
            Car key = cars.get(i);
            int j = i - 1;
            while (j >= 0 && cars.get(j).getPrice() > key.getPrice()) {
                cars.set(j + 1, cars.get(j));
                j--;
            }
            cars.set(j + 1, key);
        }
    }

    public double weightedAverage() {
        if (cars.isEmpty()) {
            return -1.0;
        }
        int weight = 1;
        int sum = 0;
        for (Car car : cars) {
            sum += car.getPrice() * weight;
            weight++;
        }
        return sum / (weight - 1);
    }

    public Car rentCheapest() {
        if (cars.isEmpty()) {
            return null;
        }
        insertionSort();
        Car cheapest = cars.get(0);
        cars.remove(0);
        return cheapest;
    }

    public ArrayList<Car> sale() {
        ArrayList<Car> saleCars = new ArrayList<>();
        Random random = new Random();
        for (Car car : cars) {
            if (random.nextBoolean()) {
                car.decreasePrice();
                saleCars.add(car);
            }
        }
        return saleCars;
    }
}