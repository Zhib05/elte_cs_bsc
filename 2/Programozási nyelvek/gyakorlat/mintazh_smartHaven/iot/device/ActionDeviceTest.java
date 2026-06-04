package iot.device;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

import iot.util.BaseModel;
import iot.util.UnSupportedDevice;

public class ActionDeviceTest {
    @ParameterizedTest
    @CsvSource({
        "TurnOn, Power on the device, Performed: TurnOn -> Power on the device",
        "TurnOff, Power off the device, Performed: TurnOff -> Power off the device",
        "Reboot, , Unknown action: Reboot"
    })
    public void testPerformActionWithParameters(String actionType, String expectedDetails, String expectedOutput) {
        try {
            ActionDevice device = new ActionDevice("TV1-ADx", Category.TV);
            if (expectedDetails != null) {
                device.addAction(actionType, expectedDetails);
            }
            assertEquals(expectedOutput, device.performAction(actionType));
        } catch (UnSupportedDevice e) {
            fail("Unexpected exception: " + e.getMessage());
        }
    }

    @Test
    public void testToggleState() {
        try {
            ActionDevice device = new ActionDevice("TV1-ADx", Category.TV);
            device.addAction("OpenChannel", "Channel4;Decrease Volume");
            device.addAction("TurnOff", "Power off the device");
            
            String expected = "Performed: OpenChannel -> Channel4;Decrease Volume\nPerformed: TurnOff -> Power off the device";
            assertEquals(expected, device.toggleState());
            assertTrue(device.getPowerStatus());
        } catch (UnSupportedDevice e) {
            fail("Unexpected exception: " + e.getMessage());
        }
    }
}