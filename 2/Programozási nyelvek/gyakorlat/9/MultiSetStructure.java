public class MultiSetStructure {
    private HasgMap<E, Integer> data;

    public MultiSetStructure(E ...elems) {
        this.data = new HashMap<E, Integer>();
        for (E elem : elems) {
            this.add(elem);
        }
    }

    public Integer add(E elem) {
        this.data.put(elem, this.data.getOrDefault(elem, 0) + 1);
        return this.data.get(elem);
    }

    public Integer getCount(E elem) {
        return this.data.getOrDefault(elem, 0);
    }

    public 
}