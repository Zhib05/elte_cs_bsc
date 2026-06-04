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
import iot.device.InteractiveDevice;
import iot.device.ActionDevice;
import iot.util.UnSupportedDevice;

public class InteractiveDeviceTest {
    @Test
    public void testBulkLinkAnyType() {
        try {
            InteractiveDevice device1 = new InteractiveDevice("led1-IDx", Category.LIGHT);
            InteractiveDevice device2 = new InteractiveDevice("led2-IDx", Category.LED_STRIP);
            ActionDevice device3 = new ActionDevice("TV1-ADx", Category.TV);

            device1.bulkLinkAnyType(device2, device3);
            assertEquals(2, device1.getConnectionLink().size());
            assertTrue(device1.getConnectionLink().contains(device2));
            assertTrue(device1.getConnectionLink().contains(device3));
        } catch (UnSupportedDevice e) {
            fail("Unexpected exception: " + e.getMessage());
        }
    }

    @Test
    public void testBulkLinkSameType() {
        try {
            InteractiveDevice device1 = new InteractiveDevice("led1-IDx", Category.LIGHT);
            InteractiveDevice device2 = new InteractiveDevice("led2-IDx", Category.LIGHT);
            ActionDevice device3 = new ActionDevice("TV1-ADx", Category.TV);

            device1.bulkLinkSameType(device2, device3);
            assertEquals(1, device1.getConnectionLink().size());
            assertTrue(device1.getConnectionLink().contains(device2));
            assertFalse(device1.getConnectionLink().contains(device3));
        } catch (UnSupportedDevice e) {
            fail("Unexpected exception: " + e.getMessage());
        }
    }

    @Test
    public void testSyncLights() {
        try {
            InteractiveDevice device1 = new InteractiveDevice("led1-IDx", Category.LIGHT);
            InteractiveDevice device2 = new InteractiveDevice("led2-IDx", Category.LED_STRIP);
            device1.linkDevice(device2);
            
            int count = device1.syncLights("blue");
            assertEquals(2, count);
            assertEquals("blue", device1.getLightColor());
            assertEquals("blue", device2.getLightColor());
        } catch (UnSupportedDevice e) {
            fail("Unexpected exception: " + e.getMessage());
        }
    }

    @Test
    public void testToggleState() {
        try {
            InteractiveDevice device = new InteractiveDevice("led1-IDx", Category.LIGHT);
            device.setLightColor("red");
            
            String result = device.toggleState();
            assertTrue(device.getPowerStatus());
            assertEquals("Device turned on", result);
            
            result = device.toggleState();
            assertEquals("Lights synchronized", result);
            assertEquals("red", device.getLightColor());
        } catch (UnSupportedDevice e) {
            fail("Unexpected exception: " + e.getMessage());
        }
    }
}