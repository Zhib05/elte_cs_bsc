package hashmapdemo;

public class StringMultiSet {
    private HashMap<String, Integer> data;

    public StringMultiSet() {
        this.data = new HashMap<String, Integer>();
    }

    public StringMultiSet(HashMap<String, Integer> initialData) {
        this.data = new HashMap<String, Integer>(initialData);
    }

    public void put(String str) {
        if (!this.data.containsKey(str)) {
            this.data.put(str, 1);
        }
        else {
            this.data.put(str, this.data.get(str) + 1);
        }
    }

    public HashMap<String, Integer> getData() {
        return new HashMap<String, Integer>(this.data);
    }
}