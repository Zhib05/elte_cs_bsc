package homework3;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class ChessPieceTest {
    @Test
    public void testGetBaseValue() {
        assertEquals(1, ChessPiece.PAWN.getBaseValue());
        assertEquals(3, ChessPiece.KNIGHT.getBaseValue());
        assertEquals(3, ChessPiece.BISHOP.getBaseValue());
        assertEquals(5, ChessPiece.ROOK.getBaseValue());
        assertEquals(9, ChessPiece.QUEEN.getBaseValue());
    }

    @Test
    public void testIsGreaterValue() {
        assertTrue(ChessPiece.KNIGHT.isGreaterValue(ChessPiece.PAWN));
        assertFalse(ChessPiece.PAWN.isGreaterValue(ChessPiece.KNIGHT));
        assertTrue(ChessPiece.ROOK.isGreaterValue(ChessPiece.BISHOP));
        assertFalse(ChessPiece.BISHOP.isGreaterValue(ChessPiece.ROOK));
        assertTrue(ChessPiece.QUEEN.isGreaterValue(ChessPiece.ROOK));
        assertFalse(ChessPiece.ROOK.isGreaterValue(ChessPiece.QUEEN));
    }
}