package data.structure;

import java.util.HashMap;

public class MultiSet<E> {
    private HashMap<E, Integer> elemToCount;

    public MultiSet(E... elems) {
        this.elemToCount = new HashMap<>();
        for (E elem : elems) {
            this.add(elem);
        }
    }

    public int add(E elem) {
        int newCount = elemToCount.getOrDefault(elem, 0) + 1;
        elemToCount.put(elem, newCount);
        return newCount;
    }

    public int getCount(E elem) {
        return elemToCount.getOrDefault(elem, 0);
    }

    public MultiSet<E> intersect(MultiSet<E> otherMultiSet) {
        MultiSet<E> newMultiSet = new MultiSet<>();
        for (E elem : this.elemToCount.keySet()) {
            if (otherMultiSet.elemToCount.containsKey(elem)) {
                int count = Math.min(this.getCount(elem), otherMultiSet.getCount(elem));
                newMultiSet.elemToCount.put(elem, count);
            }
        }
        return newMultiSet;
    }

    public int size() {
        int total = 0;
        for (int count : elemToCount.values()) {
            total += count;
        }
        return total;
    }
}