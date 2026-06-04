package zoo.people.staff;

public class Staff {
    public String name;
    public int salary;
    
    public Staff(String name, int salary) {
        this.name = name;
        this.salary = salary;
    }

    public Staff(int salary) {
        this.name = "Joe";
        this.salary = salary;
    }

    public Staff() {
        this.name = "Joe";
        this.salary = 5000;
    }
}
