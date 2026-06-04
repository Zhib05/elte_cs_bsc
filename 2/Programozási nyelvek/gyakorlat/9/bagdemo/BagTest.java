package bagdemo;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

import java.util.HashMap;
import java.util.Map.Entry;

public class BagTest {
    @Test
    public void putTest1() {
        Bag<Integer> bag = new Bag<Integer>();
        bag.put(100, 3);
        bag.put(200, 2);

        HashMap<Integer, Integer> expected = new HashMap<Integer, Integer>();
        expected.put(100, 3);
        expected.put(200, 2);

        assertEquals(expected, bag.getData());
    }

    @Test
    public void putTest2() {
        Bag<Integer> bag = new Bag<Integer>();
        bag.put(100);
        bag.put(100);
        bag.put(200);

        HashMap<Integer, Integer> expected = new HashMap<Integer, Integer>();
        expected.put(100, 2);
        expected.put(200, 1);

        assertEquals(expected, bag.getData());
    }

    @Test
    public void intersectionTest() {
        Bag<String> bag1 = new Bag<String>();
        bag1.put("apple", 3);
        bag1.put("banana", 2);
        bag1.put("bomb");

        Bag<String> bag2 = new Bag<String>();
        bag2.put("apple", 2);
        bag2.put("banana", 1);
        bag2.put("bamb");

        HashMap<String, Integer> expectedMap = new HashMap<String, Integer>();
        expectedMap.put("apple", 5);
        expectedMap.put("banana", 3);

        assertEquals(expectedMap, bag1.intersection(bag2).getData());
    }
}