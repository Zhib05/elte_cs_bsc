package iot.util;

import java.util.Objects;

import iot.device.Category;

public class BaseModel implements IotFunction {
    protected boolean powerStatus;
    protected String identifier;
    private final Category deviceType;

    public BaseModel(String identifier, Category deviceType) throws IllegalArgumentException {
        if (identifier == null || identifier.length() < 3) {
            throw new IllegalArgumentException();
        }
        this.powerStatus = false;
        this.identifier = identifier;
        this.deviceType = deviceType;
    }
    
    @Override
    public void turnOn() {
        this.powerStatus = true;
    }

    @Override
    public void turnOff() {
        this.powerStatus = false;
    }

    @Override
    public String toggleState() {
        return "BaseModel has no state";
    }
    
    public Category getDeviceType() {
        return deviceType;
    }

    public boolean getPowerStatus() {
        return powerStatus;
    }

    public String getIdentifier() {
        return identifier;
    }

    @Override
    public String toString() {
        return String.format("Device: %s, Type: %s, Identifier: %s, PowerStatus: %b", getClass().getSimpleName(), deviceType, identifier, powerStatus);
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) { return true; }
        if (!(obj instanceof BaseModel)) { return false ; }
        BaseModel other = (BaseModel) obj;

        String thisId = identifier.substring(0, 3);
        String otherId = other.identifier.substring(0, 3);

        return Objects.equals(thisId, otherId) && Objects.equals(deviceType, other.deviceType);
    }

    @Override
    public int hashCode() {
        String id = identifier.substring(0, 3);

        return Objects.hash(id, deviceType);
    }
}
