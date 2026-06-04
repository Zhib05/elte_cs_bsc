package homework3;

public enum ChessPiece{
    PAWN,
    KNIGHT,
    BISHOP,
    ROOK,
    QUEEN;

    private int baseValue;

    ChessPiece() {
        switch (this) {
            case PAWN:
                baseValue = 1;
                break;
            case KNIGHT:
                baseValue = 3;
                break;
            case BISHOP:
                baseValue = 3;
                break;
            case ROOK:
                baseValue = 5;
                break;
            case QUEEN:
                baseValue = 9;
                break;
        }
    }

    public int getBaseValue() {
        return baseValue;
    }

    public boolean isGreaterValue(ChessPiece piece) {
        return this.baseValue > piece.baseValue;
    }
}