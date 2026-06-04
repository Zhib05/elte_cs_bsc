package parking;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

import vehicle.Car;
import vehicle.Size;
import parking.facility.Gate;
import parking.facility.Space;

public class ParkingLotTest {
    @Test
    public void testConstructorWithInvalidValues() {
        assertThrows(IllegalArgumentException.class, () -> new ParkingLot(0, 0));
        assertThrows(IllegalArgumentException.class, () -> new ParkingLot(-1, 5));
        assertThrows(IllegalArgumentException.class, () -> new ParkingLot(5, -1));
    }

    @Test
    public void testTextualRepresentation() {
        ParkingLot parkingLot = new ParkingLot(2, 2);
        Gate gate = new Gate(parkingLot);
        Car car1 = new Car("ABC123", Size.SMALL, 0);
        Car car2 = new Car("DEF456", Size.LARGE, 1);
        Car car3 = new Car("GHI789", Size.SMALL, 0);
        gate.registerCar(car1);
        gate.registerCar(car2);
        gate.registerCar(car3);

        String expected = "S S L L ";
        String actual = parkingLot.toString();
        assertEquals(expected, actual);

        assertTrue(parkingLot.getFloorPlan()[0][0].isTaken());
        assertTrue(parkingLot.getFloorPlan()[0][1].isTaken());
        assertTrue(parkingLot.getFloorPlan()[1][0].isTaken());
        assertTrue(parkingLot.getFloorPlan()[1][1].isTaken());
    }
}
