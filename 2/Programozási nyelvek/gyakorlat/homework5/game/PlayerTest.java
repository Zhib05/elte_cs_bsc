package game;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class PlayerTest {
    @Test
    public void testPlayerConstructorAndGetName() {
        Player player = new Player("Steve", 100, 10);
        assertEquals("Steve", player.getName());
    }

    @Test
    void testSetName() {
        Player player = new Player("Steve", 100, 10);
        player.setName("Bob");
        assertEquals("Bob", player.getName());
    }

    @Test
    public void testAttackMonster() {
        Player player = new Player("Steve", 100, 30);
        Monster monster = new Monster(100, 20);
        int expected = monster.getHp() - (30 * 2);

        player.attack(monster);
        assertEquals(expected, monster.getHp());
    }

    @Test
    public void testAttackPlayer() {
        Player player1 = new Player("Steve", 100, 30);
        Player player2 = new Player("Bob", 100, 30);

        player1.attack(player2);
        assertEquals(100, player2.getHp());
    }
}