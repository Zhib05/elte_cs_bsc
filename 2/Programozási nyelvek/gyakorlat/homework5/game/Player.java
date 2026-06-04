package game;

public class Player extends GameCharacter {
    private String name;

    public Player(String name, int hp, int attackPower) {
        super(hp, attackPower);
        this.name = name;
        this.attackPower *= 2;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public void attack(GameCharacter enemy) {
        if (enemy instanceof Monster) {
            enemy.setHp(enemy.getHp() - this.attackPower);
            System.out.println(this.name + " damages the enemy monster for " + this.attackPower + " damage!");
        } else if (enemy instanceof Player) {
            System.out.println(this.name + " refuses to attack an ally!");
        }
    }
}