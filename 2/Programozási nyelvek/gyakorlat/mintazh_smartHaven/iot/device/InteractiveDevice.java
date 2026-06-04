package iot.device;

import iot.util.BaseModel;
import iot.util.UnSupportedDevice;

import java.util.ArrayList;

public class InteractiveDevice extends BaseModel {
    private final ArrayList<BaseModel> connectionLink;
    private String lightColor;

    public InteractiveDevice(String identifier, Category deviceType) throws UnSupportedDevice {
        super(identifier, deviceType);
        if (deviceType != Category.LED_STRIP && deviceType != Category.LIGHT) {
            throw new UnSupportedDevice("InteractiveDevice only supports LIGHT or LED_STRIP");
        }
        connectionLink = new ArrayList<>();
    }

    public String getLightColor() {
        return lightColor;
    }

    public void setLightColor(String lightColor) {
        this.lightColor = lightColor;
    }

    public ArrayList<BaseModel> getConnectionLink() {
        return connectionLink;
    }

    public void linkDevice(BaseModel dev) {
        if (!connectionLink.contains(dev)) {
            connectionLink.add(dev);
        }
    }

    public void bulkLinkSameType(BaseModel... basemodels) {
        for (BaseModel model : basemodels) {
            if (this.equals(model)) {
                linkDevice(model);
            }
        }
    }

    public void bulkLinkAnyType(BaseModel... basemodels) {
        for (BaseModel model : basemodels) {
            linkDevice(model);
        }
    }

    public int syncLights(String lightColor) {
        int count = 0;
        if (this instanceof InteractiveDevice) {
            setLightColor(lightColor);
            count++;
        }
        for (BaseModel device : connectionLink) {
            if (device instanceof InteractiveDevice) {
                ((InteractiveDevice) device).setLightColor(lightColor);
                count++;
            }
        }
        return count;
    }

    @Override
    public String toggleState() {
        if (powerStatus) {
            syncLights(lightColor);
            return "Lights synchronized";
        } else {
            turnOn();
            return "Device turned on";
        }
    }
}