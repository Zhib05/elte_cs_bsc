package quicktest6;

public class ProgrammingStudent {
    public String name;
    public ProgrammingLanguages favoriteProgrammingLanguage;
    public ForeignLanguages firstForeignLanguage;
    Animals favoriteAnimal = Animals.TIGER;

    public ProgrammingStudent(String name, ProgrammingLanguages favoriteProgrammingLanguage, ForeignLanguages firstForeignLanguage) {
        this.name = name;
        this.favoriteProgrammingLanguage = favoriteProgrammingLanguage;
        this.firstForeignLanguage = firstForeignLanguage;
    }

    public String toString() {
        return name + " likes " + favoriteProgrammingLanguage + " and" + favoriteAnimal + " who speaks " + firstForeignLanguage;
    }
}