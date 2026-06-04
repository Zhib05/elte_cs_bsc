package toStringDemo;

public class Person {
    private String name;
    private int age;

    public Person(String name, int age) {
        this.name = name;
        this.age = age;
    }

    public void printPerson() {
        System.out.println(this.name + " {" + this.age + "}");
    }

    @Override
    public String toString() {
        return this.name + " {" + this.age + "}";
    }
}