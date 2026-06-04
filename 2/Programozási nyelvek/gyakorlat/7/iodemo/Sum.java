package iodemo;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileNotFoundException;
import java.io.IOException;

public class Sum {
    public static int addNumbers(String fileName) throws NullPointerException{
        int result = 0;
        BufferedReader buffReader = null;

        File file = new File(fileName);
        try {
            buffReader = new BufferedReader(new FileReader(file));
            String line;
            while ((line = buffReader.readLine()) != null) {
                String[] lineChunks = line.split(",");
                for (String numberString : lineChunks) {
                    result += Integer.parseInt(numberString);
                }
            }
        } catch (FileNotFoundException e) {
            System.out.println("File could not be found");
        } catch (IOException e) {
            System.out.println("IO error when reading file");
        } catch (NumberFormatException e) {
            System.out.println("Could not parse number");
        }

        // buffReader.close();

        return result;
    }
}