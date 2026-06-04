package game;

import static check.CheckThat.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.*;
import org.junit.jupiter.api.extension.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;
import check.*;

public class GameCharacterTest {
    private class TestGameCharacter extends GameCharacter {
        public TestGameCharacter(int hp, int attackPower) {
            super(hp, attackPower);
        }

        @Override
        public void attack(GameCharacter enemy) {
            enemy.setHp(enemy.getHp() - this.attackPower);
        }
    }

    @Test
    public void testConstructorAndGetHp() {
        GameCharacter character = new TestGameCharacter(100, 20);
        assertEquals(100, character.getHp());
    }

    @Test
    public void testSetHp() {
        GameCharacter character = new TestGameCharacter(100, 20);
        character.setHp(50);
        assertEquals(50, character.getHp());
    }

    @Test
    public void testIsDeadWhenHpZero() {
        GameCharacter character = new TestGameCharacter(0, 20);
        assertTrue(character.isDead());
    }

    @Test
    public void testIsDeadWhenHpNegative() {
        GameCharacter character = new TestGameCharacter(100, 20);
        character.setHp(-10);
        assertTrue(character.isDead());
    }

    @Test
    public void testIsNotDeadWhenHpPositive() {
        GameCharacter character = new TestGameCharacter(100, 20);
        assertFalse(character.isDead());
    }

    @Test
    public void testAttackEnemy() {
        GameCharacter character1 = new TestGameCharacter(100, 20);
        GameCharacter character2 = new TestGameCharacter(100, 10);

        character1.attack(character2);
        assertEquals(80, character2.getHp());
    }
}