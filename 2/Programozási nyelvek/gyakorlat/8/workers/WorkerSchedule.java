package workers;

import java.util.HashMap;
import java.util.HashSet;
import java.util.ArrayList;

public class WorkerSchedule {
    private HashMap<Integer, HashSet<String>> weekToWorkers;

    public WorkerSchedule() {
        this.weekToWorkers = new HashMap<Integer, HashSet<String>>();
    }

    public void add(int week, HashSet<String> workers) {
        if (!this.weekToWorkers.containsKey(week)) {
            this.weekToWorkers.put(week, new HashSet<String>());
        }

        weekToWorkers.get(week).addAll(workers);
    }

    public void add(HashSet<Integer> a, ArrayList<String> b) {

    }

    public boolean isWorkingOnWeek(String a, int b) {
        return false;
    }

    public HashSet<Integer> getWorkWeeks(String a) {
        return new HashSet<Integer>();
    }
}