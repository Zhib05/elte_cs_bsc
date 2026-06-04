package iot.device;

import iot.util.BaseModel;
import iot.util.UnSupportedDevice;

import java.util.HashMap;

public class ActionDevice extends BaseModel {
    private final HashMap<String, String> actions;
    private String lastAction;

    public ActionDevice(String identifier, Category deviceType) throws UnSupportedDevice {
        super(identifier, deviceType);
        if (deviceType == Category.LED_STRIP || deviceType == Category.LIGHT) {
            throw new UnSupportedDevice("ActionDevice can not be LED_STRIP or LIGHT");
        }
        lastAction = "None";
        actions = new HashMap<String, String>();
    }

    public HashMap<String, String> getActions() {
        return actions;
    }

    public String getLastAction() {
        return lastAction;
    }

    public void addAction(String actionType, String details) {
        actions.put(actionType, details);
    }

    public String getAvailableActions() {
        if (actions.isEmpty()) {
            return "No actions available";
        }
        return String.join(",", actions.keySet());
    }

    public String performAction(String actionType) {
        if (actions.containsKey(actionType)) {
            lastAction = actionType;
            return "Performed: " + actionType + " -> " + actions.get(actionType);
        } else {
            return "Unknown action: " + actionType;
        }
    }

    @Override
    public String toggleState() {
        if (!actions.isEmpty()) {
            turnOn();
            StringBuilder result = new StringBuilder();
            for (String actionType : actions.keySet()) {
                result.append(performAction(actionType)).append("\n");
            }
            return result.toString().trim();
        } else {
            return "No actions available";
        }
    }
}