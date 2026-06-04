class ComplexMain {
    public static void main(String[] args) {
        Complex c1 = new Complex(1.0, 2.0);
        Complex c2 = new Complex(3.0, 4.0);

        System.out.println("c1: " + c1);
        System.out.println("c2: " + c2);

        System.out.println("c1 Abszolut ertek: " + c1.abs());
        System.out.println("c2 Abszolut ertek: " + c2.abs());

        System.out.println("c1 + c2: " + c1.add(c2));
        System.out.println("c1 - c2: " + c1.sub(c2));
        System.out.println("c1 * c2: " + c1.mul(c2));
    }    
}
