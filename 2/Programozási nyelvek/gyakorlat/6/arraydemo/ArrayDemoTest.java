package arraydemo;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class ArrayDemoTest {
    @Test
    public void max1Test() {
        assertEquals(7, ArrayDemo.max1(new int[] {6, 2, 7, 5}));
        assertEquals(9, ArrayDemo.max1(new int[] {9, 2, 7, 5}));
        assertEquals(15, ArrayDemo.max1(new int[] {9, 2, 7, 15}));
        assertEquals(0, ArrayDemo.max1(new int[] {}));
    }

    @Test
    public void max2Test() {
        assertEquals(7, ArrayDemo.max2(new int[] {6, 2, 7, 5}));
        assertEquals(9, ArrayDemo.max2(new int[] {9, 2, 7, 5}));
        assertEquals(15, ArrayDemo.max2(new int[] {9, 2, 7, 15}));
        assertEquals(0, ArrayDemo.max2(new int[] {}));
    }

    @Test
    public void max3Test() {
        assertEquals(7, ArrayDemo.max3(new int[] {6, 2, 7, 5}));
        assertEquals(9, ArrayDemo.max3(new int[] {9, 2, 7, 5}));
        assertEquals(15, ArrayDemo.max3(new int[] {9, 2, 7, 15}));
        assertEquals(0, ArrayDemo.max3(new int[] {}));
    }

    @Test
    public void max4Test() {
        assertEquals(7, ArrayDemo.max4(new int[] {6, 2, 7, 5}));
        assertEquals(9, ArrayDemo.max4(new int[] {9, 2, 7, 5}));
        assertEquals(15, ArrayDemo.max4(new int[] {9, 2, 7, 15}));
        assertEquals(0, ArrayDemo.max4(new int[] {}));
    }
}