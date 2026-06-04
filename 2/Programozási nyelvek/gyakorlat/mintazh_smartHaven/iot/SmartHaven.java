package iot;

import iot.util.BaseModel;
import iot.device.ActionDevice;
import iot.device.InteractiveDevice;
import iot.util.UnSupportedDevice;
import iot.device.Category;

import java.util.ArrayList;
import java.util.HashMap;

public class SmartHaven {
    private final ArrayList<BaseModel> devices;

    public SmartHaven(int n) {
        Supplier supplier = new Supplier();
        devices = supplier.bringSupplies(n);
    }

    public ArrayList<BaseModel> getDevices() {
        return devices;
    }

    public void turnOnAllDevices() {
        for (BaseModel device : devices) {
            device.turnOn();
        }
    }

    public void turnOffAllDevices() {
        for (BaseModel device : devices) {
            device.turnOff();
        }
    }

    public void toggleAllDevices() {
        for (BaseModel device : devices) {
            device.toggleState();
        }
    }

    public void printAvailableActions() {
        for (BaseModel device : devices) {
            if (device instanceof ActionDevice) {
                ActionDevice actionDevice = (ActionDevice) device;
                System.out.println(device.getIdentifier() + ":");
                for (HashMap.Entry<String, String> entry : actionDevice.getActions().entrySet()) {
                    System.out.println("  " + entry.getKey() + " -> " + entry.getValue());
                }
            }
        }
    }

    public void executeDeviceActions() {
        for (BaseModel device : devices) {
            if (device instanceof ActionDevice) {
                ((ActionDevice) device).performAction("activate");
                ((ActionDevice) device).performAction("start");
            }
        }
    }

    public void toggleLightsForInteractiveDevices() {
        for (BaseModel device : devices) {
            if (device instanceof InteractiveDevice) {
                ((InteractiveDevice) device).toggleState();
            }
        }
    }

    public int syncLightsForInteractiveDevices(String lightColor) {
        int count = 0;
        for (BaseModel device : devices) {
            if (device instanceof InteractiveDevice) {
                count += ((InteractiveDevice) device).syncLights(lightColor);
            }
        }
        return count;
    }

    public void linkDevicesAfterNth(int n) {
        ArrayList<InteractiveDevice> interactiveDevice = new ArrayList<>();
        InteractiveDevice nthDevice = null;
        int found = 0;

        for (BaseModel device : devices) {
            if (device instanceof InteractiveDevice) {
                interactiveDevice.add((InteractiveDevice) device);
                found++;
                if (found == n) {
                    nthDevice = (InteractiveDevice) device;
                }
            }
        }

        if (nthDevice != null) {
            boolean startLinking = false;
            for (InteractiveDevice device : interactiveDevice) {
                if (startLinking && device.getDeviceType() == nthDevice.getDeviceType()) {
                    nthDevice.linkDevice(device);
                }
                if (device == nthDevice) {
                    startLinking = true;
                }
            }
        }
    }
}