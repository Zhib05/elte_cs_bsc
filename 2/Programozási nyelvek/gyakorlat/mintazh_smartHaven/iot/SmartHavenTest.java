package iot;

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

public class SmartHavenTest {
    @Test
    public void testToggleAllDevices() {
        SmartHaven haven = new SmartHaven(3);
        haven.toggleAllDevices();

        boolean allPoweredOn = true;
        for (BaseModel device : haven.getDevices()) {
            if (!device.getPowerStatus()) {
                allPoweredOn = false;
                break;
            }
        }
        assertTrue(allPoweredOn);
    }

    @Test
    public void testToggleLightsForInteractiveDevices() {
        SmartHaven haven = new SmartHaven(3);
        haven.toggleLightsForInteractiveDevices();

        boolean allInteractivePoweredOn = true;
        for (BaseModel device : haven.getDevices()) {
            if (device instanceof InteractiveDevice && !device.getPowerStatus()) {
                allInteractivePoweredOn = false;
                break;
            }
        }

        assertTrue(allInteractivePoweredOn);
    }

    @Test
    public void testSyncLightsForInteractiveDevices() {
        SmartHaven haven = new SmartHaven(3);
        int count = haven.syncLightsForInteractiveDevices("blue");
        assertEquals(2, count);

        boolean allBlue = true;
        for (BaseModel device : haven.getDevices()) {
            if (device instanceof InteractiveDevice) {
                if (!((InteractiveDevice) device).getLightColor().equals("blue")) {
                    allBlue = false;
                    break;
                }
            }
        }

        assertTrue(allBlue);
    }

    @Test
    public void testLinkDevicesAfterNth() {
        SmartHaven haven = new SmartHaven(4);
        haven.linkDevicesAfterNth(1);

        InteractiveDevice firstInteractive = null;
        for (BaseModel device : haven.getDevices()) {
            if (device instanceof InteractiveDevice) {
                firstInteractive = (InteractiveDevice) device;
                break;
            }
        }
        assertEquals(1, firstInteractive.getConnectionLink().size());
        assertNotNull(firstInteractive);
    }
}