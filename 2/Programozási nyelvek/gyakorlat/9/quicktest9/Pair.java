package quicktest9;

public class Pair<T, U> {
    private T item1;
    private U itme2;

    public Pair(T item1, U item2) {
        this.item1 = item1;
        this.itme2 = item2;
    }

    public T getItem1() {
        return item1;
    }

    public U getItem2() {
        return itme2;
    }

    public void setItem1(T item1) {
        this.item1 = item1;
    }

    public void setItem2(U item2) {
        this.itme2 = item2;
    }
}