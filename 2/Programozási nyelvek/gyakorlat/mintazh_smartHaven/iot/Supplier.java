package iot;

import iot.util.BaseModel;
import iot.device.ActionDevice;
import iot.device.InteractiveDevice;
import iot.util.UnSupportedDevice;
import iot.device.Category;

import java.util.ArrayList;

public class Supplier {
    public Supplier() {}

    public static ArrayList<BaseModel> bringSupplies(int n) {
        ArrayList<BaseModel> supplies = new ArrayList<>();
        int j = 0;
        int id = 1;
        int ad = 1;

        for (int i = 0; i < n; ++i) {
            try {
                switch (i % 3) {
                    case 0:
                        supplies.add(new InteractiveDevice("led" + id++ + "-IDx", Category.LIGHT));
                        break;
                    case 1:
                        supplies.add(new InteractiveDevice("led" + id++ + "-IDx", Category.LED_STRIP));
                        break;
                    case 3:
                        if (j == 0) {
                            supplies.add(new ActionDevice("TV" + ad++ + "-ADx", Category.TV));
                            ++j;
                            break;
                        } else if (j == 1) {
                            supplies.add(new ActionDevice("FRIDGE" + ad++ + "-ADx", Category.FRIDGE));
                            ++j;
                            break;
                        } else if (j == 2) {
                            supplies.add(new ActionDevice("COFFEE_MACHINE" + ad++ + "-ADx", Category.COFFEE_MACHINE));
                            j = 0;
                            break;
                        }
                }
            } catch (UnSupportedDevice e) {

            }
        }
        return supplies;
    }
}