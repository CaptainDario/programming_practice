/*
 * 
 * 
 */

public class UseNet_Exception {

	public static void main(String[] args) {
		//Erstellen der Resistoren für das Netz
		Resistor R1 = new Resistor(100, "R1");
		Resistor R2 = new Resistor(200, "R2");
		Resistor R3 = new Resistor(300, "R3");
		Resistor R4 = new Resistor(400, "R4");
		Resistor R5 = new Resistor(500, "R5");
		Resistor R6 = new Resistor(600, "R6");
		
		//Exception Resistoren
		Resistor R7 = new Resistor(100, "R1");
		Resistor R8 = new Resistor(100, "R2");
		Resistor R9 = new Resistor(100, "R3");
		
		//Netze erstellen
		Parallel R1R3 = new Parallel(R1, R3);
		Serial R1R2R3 = new Serial(R1R3, R2);
		Serial R4R5 = new Serial(R4, R5);
		Parallel R4R5R6 = new Parallel(R4R5, R6);
		Parallel R1R2R3R4R5R6 = new Parallel(R1R2R3, R4R5R6);
		
		//EXCEPTIONS
		Parallel R7R8 = new Parallel(R7, R8);
		Parallel R7R8R9 = new Parallel(R7R8, R9);
		Parallel R1R2R3R4R5R6R7R8R9 = new Parallel(R1R2R3R4R5R6, R7R8R9);
		
		//Ausgabe wegen Exception blockiert
		System.out.println(R1R2R3R4R5R6R7R8R9.show());
	}
	
}
