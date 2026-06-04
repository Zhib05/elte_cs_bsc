package enumdemo;

public class TrafficLigth {
    Color color = Color.RED;

    public void signal() {
        // ha piors: STOP
        // ha sárga: Siessen
        // ha zöld: Mehet
        switch (this.color) {
            case Color.RED: System.out.println("STOP"); break;
            case Color.YELLOW: System.out.println("HURRY UP"); break;
            case Color.GREEN: System.out.println("GO"); break;
        }
    }
}