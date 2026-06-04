package parking.facility;

import vehicle.Car;
import vehicle.Size;
import parking.ParkingLot;
import parking.facility.Space;
import java.util.ArrayList;
import java.util.List;

public class Gate {
    private final ArrayList<Car> cars;
    private final ParkingLot parkingLot;
    private int ticketCounter;

    public Gate(ParkingLot parkingLot) {
        this.cars = new ArrayList<>();
        this.parkingLot = parkingLot;
        this.ticketCounter = 0;
    }

    private Space findTakenSpaceByCar(Car car) {
        Space[][] floorPlan = parkingLot.getFloorPlan();
        for (int row = 0; row < parkingLot.getRows(); row++) {
            for (int col = 0; col < parkingLot.getCols(); col++) {
                Space currentSpace = floorPlan[row][col];
                if (currentSpace.isTaken() && currentSpace.getOccupyingCar() == car) {
                    return currentSpace;
                }
            }
        }
        return null;
    }

    private Space findAvailableSpaceOnFloor(int floor, Car car) {
        Space[][] floorPlan = parkingLot.getFloorPlan();
        for (int col = 0; col < parkingLot.getCols(); col++) {
            if (!(floorPlan[floor][col].isTaken())) {
                if (car.getSpotOccupation() == Size.SMALL) { 
                    return floorPlan[floor][col];
                }
                else if (car.getSpotOccupation() == Size.LARGE && col + 1 < parkingLot.getCols() && !floorPlan[floor][col + 1].isTaken()) {
                    return floorPlan[floor][col + 1];
                }
            }
        }
        return null;
    }

    public Space findAnyAvailableSpaceForCar(Car car) {
        for (int i = 0; i < parkingLot.getRows(); i++) {
            Space space = findAvailableSpaceOnFloor(i, car);
            return space;
        }
        return null;
    }

    public Space findPreferredAvailableSpaceForCar(Car car) {
        int preferredFloor = car.getPreferredFloor();
        Space space = findAvailableSpaceOnFloor(preferredFloor, car);
        if (space != null) {
            return space;
        }

        for (int i = preferredFloor - 1; i >= 0; i--) {
            space = findAvailableSpaceOnFloor(i, car);
            if (space != null) {
                return space;
            }
        }

        for (int i = preferredFloor + 1; i < parkingLot.getRows(); i++) {
            space = findAvailableSpaceOnFloor(i, car);
            if (space != null) {
                return space;
            }
        }
        return null;
    }

    public boolean registerCar(Car car) {
        Space space = findPreferredAvailableSpaceForCar(car);
        if (space == null) {
            return false;
        }

        int floor = space.getFloorNumber();
        int cols = space.getSpaceNumber();
        Space[][] floorPlan = parkingLot.getFloorPlan();

        String ticketId = "Ticket_" + ticketCounter++;
        car.setTicketId(ticketId);

        if (car.getSpotOccupation() == Size.SMALL) {
            floorPlan[floor][cols].addOccupyingCar(car);
        } else {
            floorPlan[floor][cols].addOccupyingCar(car);
            floorPlan[floor][cols - 1].addOccupyingCar(car);
        }
        return true;
    }

    public void registerCars(Car[] cars) {
        for (Car car : cars) {
            if (!registerCar(car)) {
                System.err.println("Nem található hely az autó számára: " + car);
            }
        }
    }

    public void deRegisterCar(String ticketId) {
        for (int i = 0; i < cars.size(); i++) {
            Car car = cars.get(i);
            if (car.getTicketId().equals(ticketId)) {
                Space space = findTakenSpaceByCar(car);
                if (space != null) {
                    space.removeOccupyingCar();
                    cars.remove(i);
                }
            }
        }
    }
}