package parking;

import parking.facility.Space;
import vehicle.Car;
import vehicle.Size;

public class ParkingLot {
    private final Space[][] floorPlan;
    private int rows;
    private int cols;

    public ParkingLot(int floorNumber, int spaceNumber) {
        if (floorNumber <= 0 || spaceNumber <= 0) {
            throw new IllegalArgumentException("Floor number and space number must be greater than 0.");
        }

        this.rows = floorNumber;
        this.cols = spaceNumber;
        this.floorPlan = new Space[rows][cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                floorPlan[i][j] = new Space(i, j);
            }
        }
    }

    public Space[][] getFloorPlan() {
        return floorPlan;
    }

    public int getRows() {
        return rows;
    }

    public int getCols() {
        return cols;
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                Space space = floorPlan[i][j];
                if (space.getOccupyingCarSize() == Size.SMALL) {
                    sb.append("S ");
                } else if (space.getOccupyingCarSize() == Size.LARGE) {
                    sb.append("L ");
                } else {
                    sb.append("X ");
                }
            }
        }
        return sb.toString();
    }
}