package data.structure;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

import java.util.HashMap;

public class MultiSetTest {
    @Test
    public void multiSetInteger() {
        MultiSet<Integer> multiSetInt = new MultiSet<>(1, 2, 2, 3, 4);
        assertEquals(1, multiSetInt.getCount(1));
        assertEquals(2, multiSetInt.getCount(2));
        assertEquals(1, multiSetInt.getCount(3));
        assertEquals(1, multiSetInt.getCount(4));

        multiSetInt.add(3);
        assertEquals(2, multiSetInt.getCount(3));
    }

    @Test
    public void multiSetString() {
        MultiSet<String> multiSetString = new MultiSet<>("alma", "banan", "banan");
        assertEquals(1, multiSetString.getCount("alma"));
        assertEquals(2, multiSetString.getCount("banan"));

        multiSetString.add("szilva");
        multiSetString.add("banan");
        assertEquals(3, multiSetString.getCount("banan"));
        assertEquals(1, multiSetString.getCount("szilva"));
    }
}