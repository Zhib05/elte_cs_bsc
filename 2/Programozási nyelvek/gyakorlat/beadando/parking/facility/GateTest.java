package parking.facility;

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
import parking.ParkingLot;
import parking.facility.Space;
import java.util.ArrayList;
import java.util.List;

public class GateTest {
    @Test
    public void testFindAnyAvailableSpaceForCar() {
        ParkingLot parkingLot = new ParkingLot(2, 2);
        Gate gate = new Gate(parkingLot);
        Car car1 = new Car("ABC123", Size.SMALL, 0);
        Car car2 = new Car("DEF456", Size.LARGE, 1);
        Space space1 = gate.findAnyAvailableSpaceForCar(car1);
        Space space2 = gate.findAnyAvailableSpaceForCar(car2);
        assertEquals(0, space1.getFloorNumber());
        assertEquals(0, space1.getSpaceNumber());
        assertEquals(0, space2.getFloorNumber());
        assertEquals(1, space2.getSpaceNumber());

        space1.addOccupyingCar(car1);
        space2.addOccupyingCar(car2);
        assertTrue(space1.isTaken());
        assertTrue(space2.isTaken());
    }

    @ParameterizedTest(name = "{0} {1} {2} ⟹ {3}:{4}")
    @CsvSource(textBlock = """
        ABC123, SMALL, 0, 0, 0
        DEF456, LARGE, 1, 1, 0
        JKL012, SMALL, 1, 1, 1
    """)
    @DisableIfHasBadStructure
    public void testFindPreferredAvailableSpaceForCar(String Plate, Size size, int preferredFloor) {
        ParkingLot parkingLot = new ParkingLot(2, 2);
        Gate gate = new Gate(parkingLot);
        Car car = new Car(Plate, size, preferredFloor);
        Space space = gate.findPreferredAvailableSpaceForCar(car);
        assertEquals(preferredFloor, space.getFloorNumber());
        if (size == Size.LARGE) {
            assertEquals(1, space.getSpaceNumber());
        } else {
            assertEquals(0, space.getSpaceNumber());
        }
        assertNotNull(space);
    }


    @ParameterizedTest(name = "{0} {1} {2} ⟹ {3}:{4}")
    @CsvSource(textBlock = """
        ABC123, SMALL, 0, 0, 0
        DEF456, LARGE, 1, 1, 0
        JKL012, SMALL, 1, 1, 1
    """)
    @DisableIfHasBadStructure
    public void testRegisterCar(String Plate, Size size, int preferredFloor) {
        ParkingLot parkingLot = new ParkingLot(2, 2);
        Gate gate = new Gate(parkingLot);
        Car car = new Car(Plate, size, preferredFloor);
        boolean isRegistered = gate.registerCar(car);
        assertTrue(isRegistered);
        assertTrue(car.getTicketId() != null);
    }

    @ParameterizedTest(name = "{0} {1} {2} ⟹ {3}:{4}")
    @CsvSource(textBlock = """
        ABC123, SMALL, 0, 0, 0
        DEF456, LARGE, 1, 1, 0
        JKL012, SMALL, 1, 1, 1
    """)
    @DisableIfHasBadStructure
    public void testDeRegisterCar(String Plate, Size size, int preferredFloor) {
        ParkingLot parkingLot = new ParkingLot(2, 2);
        Gate gate = new Gate(parkingLot);
        Car car = new Car(Plate, size, preferredFloor);
        Space space = gate.findPreferredAvailableSpaceForCar(car);
        assertNotNull(space);
        space.addOccupyingCar(car);
        assertTrue(space.isTaken());
        space.removeOccupyingCar();
        assertFalse(space.isTaken());
        assertNull(space.getOccupyingCar());
    }

    // @Test
    // public void testFindPreferredAvailableSpaceForCar(String Plate, Size size, int preferredFloor) {
    //     ParkingLot parkingLot = new ParkingLot(2, 2);
    //     Gate gate = new Gate(parkingLot);
    //     Car car = new Car(Plate, size, preferredFloor);
    //     Space space = gate.findPreferredAvailableSpaceForCar(car);
    //     assertEquals(preferredFloor, space.getFloorNumber());
    //     assertEquals(0, space.getSpaceNumber());
    //     if (size == Size.LARGE) {
    //         assertEquals(1, space.getSpaceNumber());
    //     } else {
    //         assertEquals(0, space.getSpaceNumber());
    //     }
    // }

    // @Test
    // public void testRegisterCar(String Plate, Size size, int preferredFloor) {
    //     ParkingLot parkingLot = new ParkingLot(2, 2);
    //     Gate gate = new Gate(parkingLot);
    //     Car car = new Car(Plate, size, preferredFloor);
    //     Space space = gate.findPreferredAvailableSpaceForCar(car);
    //     assertNotNull(space);
    //     space.addOccupyingCar(car);
    //     assertTrue(space.isTaken());
    // }

    // @Test
    // public void testDeRegisterCar(String Plate, Size size, int preferredFloor) {
    //     ParkingLot parkingLot = new ParkingLot(2, 2);
    //     Gate gate = new Gate(parkingLot);
    //     Car car = new Car(Plate, size, preferredFloor);
    //     Space space = gate.findPreferredAvailableSpaceForCar(car);
    //     assertNotNull(space);
    //     space.addOccupyingCar(car);
    //     assertTrue(space.isTaken());
    //     space.removeOccupyingCar();
    //     assertFalse(space.isTaken());
    //     assertNull(space.getOccupyingCar());
    // }
}