package game;

public class Monster extends GameCharacter {
    public Monster(int hp, int attackPower) {
        super(hp, attackPower);
    }

    @Override
    public void attack(GameCharacter enemy) {
        enemy.setHp(enemy.getHp() - this.attackPower);
        System.out.println("The monster damages it's enemy for " + this.attackPower + " damage!");
    }
}