package homework4;

import java.util.ArrayList;
import java.util.List;

public class Stack<T> {
    private final List<T> stack;

    public Stack() {
        this.stack = new ArrayList<>();
    }

    public void push(T item) {
        stack.add(item);
    }

    public T pop() throws IllegalStateException {
        if (empty()) {
            throw new IllegalStateException("Cannot pop from an empty stack");
        }
        return stack.remove(stack.size() - 1);
    }

    public T peek() throws IllegalStateException {
        if (empty()) {
            throw new IllegalStateException("Cannot peek an empty stack");
        }
        return stack.get(stack.size() - 1);
    }

    public boolean empty() {
        return stack.isEmpty();
    }
}