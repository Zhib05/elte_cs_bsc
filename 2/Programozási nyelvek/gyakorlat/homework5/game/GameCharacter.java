package game;

public abstract class GameCharacter {
    private int hp;
    protected int attackPower;

    public GameCharacter(int hp, int attackPower) {
        this.hp = hp;
        this.attackPower = attackPower;
    }

    public int getHp() {
        return hp;
    }

    public void setHp(int hp) {
        this.hp = hp;
    }

    public abstract void attack(GameCharacter enemy);

    public boolean isDead() {
        if (hp <= 0) {
            return true;
        } else {
            return false;
        }
    }
}