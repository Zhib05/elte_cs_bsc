package parking.facility;

import vehicle.Car;
import vehicle.Size;

public class Space{
    private Car occupyingCar;
    private final int floorNumber;
    private final int spaceNumber;

    public Space(int floorNumber, int spaceNumber) {
        this.floorNumber = floorNumber;
        this.spaceNumber = spaceNumber;
        this.occupyingCar = null;
    }

    public int getFloorNumber() {
        return floorNumber;
    }

    public int getSpaceNumber() {
        return spaceNumber;
    }

    public String getCarLicensePlate() {
        return occupyingCar.getLicensePlate();
    }

    public Size getOccupyingCarSize() {
        return occupyingCar.getSpotOccupation();
    }

    public void addOccupyingCar(Car car) {
        this.occupyingCar = car;
    }

    public void removeOccupyingCar() {
        this.occupyingCar = null;
    }

    public boolean isTaken() {
        return occupyingCar != null;
    }

    public Car getOccupyingCar() {
        return occupyingCar;
    }
}