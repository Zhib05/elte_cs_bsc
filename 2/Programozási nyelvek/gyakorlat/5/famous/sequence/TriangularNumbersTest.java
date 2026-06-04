package famous.sequence;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class TriangularNumbersTest {
    @Test
    public void trivialTestCase() {
        assertEquals(0, TriangularNumbers.getTriangularNumber(0));
        assertEquals(1, TriangularNumbers.getTriangularNumber(1));
        assertEquals(100, TriangularNumbers.getTriangularNumber(14));
        assertEquals(-1, TriangularNumbers.getTriangularNumber(-1));
        assertEquals(-3, TriangularNumbers.getTriangularNumber(-2));
    }
}