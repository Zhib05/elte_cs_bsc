import java.util.ArrayList;
import java.util.List;

// 1. Az Interfész
interface SalariedEntity {
    double getSalary();
}

// 2. Az Absztrakt Employee osztály
abstract class Employee implements SalariedEntity {
    private String name;
    private double salary;

    public Employee(String name, double salary) {
        this.name = name;
        this.salary = salary;
    }

    public String getName() {
        return name;
    }

    // Segédmetódus, hogy a leszármazottak hozzáférjenek a privát fizetéshez
    protected double getBaseSalary() {
        return salary;
    }

    // A fizetésemelő metódus (százalékos)
    public void raiseSalary(double percentage) {
        this.salary += this.salary * (percentage / 100.0);
    }

    // Absztrakt metódus lett az interfész és a feladat leírása alapján
    @Override
    public abstract double getSalary();
}

// 3. Subordinate (Beosztott) osztály
class Subordinate extends Employee {
    
    public Subordinate(String name, double salary) {
        super(name, salary);
    }

    // Az eredeti működés: egyszerűen visszaadja az alapfizetést
    @Override
    public double getSalary() {
        return getBaseSalary();
    }
}

// 4. Manager osztály
class Manager extends Employee {
    private List<Employee> subordinates;

    public Manager(String name, double salary) {
        super(name, salary);
        this.subordinates = new ArrayList<>();
    }

    public void addEmployee(Employee e) {
        subordinates.add(e);
    }

    public void removeEmployee(Employee e) {
        subordinates.remove(e);
    }

    // Saját fizetés + a beosztottak fizetésének 5%-a
    @Override
    public double getSalary() {
        double totalSalary = getBaseSalary();
        for (Employee e : subordinates) {
            totalSalary += e.getSalary() * 0.05;
        }
        return totalSalary;
    }
}

// 5. Subcontractor (Alvállalkozó) osztály
class Subcontractor implements SalariedEntity {
    private long taxNumber;
    private double fee; // Szerződéses díj / fizetés

    public Subcontractor(long taxNumber, double fee) {
        this.taxNumber = taxNumber;
        this.fee = fee;
    }

    public long getTaxNumber() {
        return taxNumber;
    }

    @Override
    public double getSalary() {
        return fee;
    }
}

class Company {
    private List<SalariedEntity> entities;

    public Company () {
        entities = new ArrayList<>();
    }

    public void addEntity(SalariedEntity entity) {
        entities.add(entity);
    }

    public void removeEntity(SalariedEntity entity) {
        entities.remove(entity);
    }

    public void employeeRaiseSalary(double percentage) {
        for (SalariedEntity entity : entities) {
            if (entity instanceof Employee) {
                ((Employee) entity).raiseSalary(percentage);
            }
        }
    }
}

// 6. A tesztelő Főosztály
public class Review {
    public static void main(String[] args) {
        // Alkalmazottak létrehozása
        Subordinate emp1 = new Subordinate("Kovács Péter", 300000);
        Subordinate emp2 = new Subordinate("Nagy Anna", 350000);
        
        Manager manager = new Manager("Tóth Gábor", 600000);
        
        // Beosztottak hozzárendelése a menedzserhez
        manager.addEmployee(emp1);
        manager.addEmployee(emp2);
        
        // Alvállalkozó létrehozása
        Subcontractor sub = new Subcontractor(123456789L, 400000);

        // Alapállapot kiírása
        System.out.println("--- Eredeti fizetések ---");
        System.out.println(emp1.getName() + " fizetése: " + emp1.getSalary());
        System.out.println(emp2.getName() + " fizetése: " + emp2.getSalary());
        System.out.println(manager.getName() + " fizetése: " + manager.getSalary());
        System.out.println("Alvállalkozó (" + sub.getTaxNumber() + ") fizetése: " + sub.getSalary());
        
        // Fizetésemelés tesztelése
        System.out.println("\n--- 10% fizetésemelés a beosztottaknak ---");
        emp1.raiseSalary(10); // 300.000 -> 330.000
        emp2.raiseSalary(10); // 350.000 -> 385.000
        
        // Ellenőrizzük, hogyan változott a menedzser fizetése
        // (A menedzser bónusza is nőnie kell, hiszen a beosztottak fizetése nőtt)
        System.out.println(emp1.getName() + " új fizetése: " + emp1.getSalary());
        System.out.println(emp2.getName() + " új fizetése: " + emp2.getSalary());
        System.out.println(manager.getName() + " új fizetése: " + manager.getSalary());
    }
}