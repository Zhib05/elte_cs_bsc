package point2d;

public class Point {
    private double x;
    private double y;

    public Point(double x, double y) {
        this.x = x;
        this.y = y;
    }

    public void move(double dx, double dy) {
        this.x += dx;
        this.y += dy;
    }

    public void mirror(double cx, double cy) {
        double dx = 2 * (cx - this.x);
        double dy = 2 * (cy - this.y);
        this.move(dx, dy);
    }

    public void mirror(Point other) { // method overloading(a fordito magatol kitalalja hogz mikor melyik fuggvenyt kell haznalni)
        this.mirror(other.x, other.y);
    }

    public double distance(Point other) {
        // return Math.sqrt((other.x - this.x) * (other.x - this.x) + (other.y - this.y) * (other.y - this.y));
        return Math.sqrt(Math.pow(other.x - this.x, 2) + Math.pow(other.y - this.y, 2));
    }

    // GETTERS
    public double getX() {
        return this.x;
    }

    public double getY() {
        return this.y;
    }

    // SETTERS
    public void setX(double newX) {
        if (newX > 0) {
            this.x = newX;
        } else {
            System.err.println("Invalid x coordinate");
        }
    }
}

