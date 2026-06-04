package homework4;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class StackTest {
    @Test
    public void testPush() {
        Stack<Integer> stack = new Stack<>();
        stack.push(1);
        assertEquals(1, stack.peek());
        stack.push(2);
        assertEquals(2, stack.peek());
    }

    @Test
    public void testPop() {
        Stack<Integer> stack = new Stack<>();
        stack.push(1);
        stack.push(2);
        assertEquals(2, stack.pop());
        assertEquals(1, stack.peek());
    }

    @Test
    public void testPeek() {
        Stack<Integer> stack = new Stack<>();
        stack.push(1);
        stack.push(2);
        assertEquals(2, stack.peek());
        stack.pop();
        assertEquals(1, stack.peek());
    }

    @Test
    public void testEmpty() {
        Stack<Integer> stack = new Stack<>();
        assertTrue(stack.empty());
        stack.push(1);
        assertFalse(stack.empty());
        stack.pop();
        assertTrue(stack.empty());
    }
}