package vehicle;

public class Car{
    private final Size spotOccupation;
    private final String licensePlate;
    private String ticketId;
    private int preferredFloor;

    public Car(String licensePlate, Size spotOccupation, int preferredFloor) {
        this.spotOccupation = spotOccupation;
        this.licensePlate = licensePlate;
        this.ticketId = null;
        this.preferredFloor = preferredFloor;
    }

    public String getTicketId() {
        return ticketId;
    }

    public void setTicketId(String ticketId) {
        this.ticketId = ticketId;
    }

    public String getLicensePlate() {
        return licensePlate;
    }

    public int getPreferredFloor() {
        return preferredFloor;
    }

    public void setPreferredFloor(int preferredFloor) {
        this.preferredFloor = preferredFloor;
    }

    public Size getSpotOccupation() {
        return spotOccupation;
    }
}