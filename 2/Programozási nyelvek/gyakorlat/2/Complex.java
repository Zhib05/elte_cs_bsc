class Complex {
    private double real;
    private double imag;

    public Complex(double real, double imag) {
        this.real = real;
        this.imag = imag;
    }

    public double abs() {
        return Math.sqrt(real * real + imag * imag);
    }

    public Complex add(Complex c) {
        return new Complex(this.real + c.real, this.imag + c.imag);
    }

    public Complex sub(Complex c) {
        return new Complex(this.real - c.real, this.imag - c.imag);
    }

    public Complex mul(Complex c) {
        return new Complex(this.real * c.real - this.imag * c.imag, this.real * c.imag + this.imag * c.real);
    }

    // @Override
    @Override public String toString() {
        return "(" + real + " + " + imag + "i" + ")";
    }
}
