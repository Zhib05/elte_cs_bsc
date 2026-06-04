package game;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class MonsterTest {
    @Test
    public void testMonsterConstructorAndGetters() {
        Monster monster = new Monster(100, 20);
        assertEquals(100, monster.getHp());
        assertFalse(monster.isDead());
    }

    @Test
    public void testAttackEnemy() {
        Monster monster = new Monster(100, 20);
        Player player = new Player("Steve", 100, 30);

        monster.attack(player);
        assertEquals(80, player.getHp());
    }

    @Test
    public void testKillsEnemy() {
        Monster monster = new Monster(100, 50);
        Player player = new Player("Steve", 100, 30);

        monster.attack(player);
        monster.attack(player);
        assertTrue(player.isDead());
        assertEquals(0, player.getHp());
    }
}