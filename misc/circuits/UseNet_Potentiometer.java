/*
 * 
 * 
 */

public class UseNet_Potentiometer {

	public static void main(String[] args) {
		//Erstellen der Resistoren für das Netz
		Resistor R1 = new Resistor(100, "R1");
		Resistor R2 = new Resistor(200, "R2");
		Resistor R3 = new Resistor(300, "R3");
		Potentiometer R4 = new Potentiometer("R4");
		Resistor R5 = new Resistor(500, "R5");
		Resistor R6 = new Resistor(600, "R6");
		
		
		//Schleife die den Widerstand für das Potentionmeter regelt
		for(double i = 400; i <= 5000 ; i += 200)
		{
			//setzen des Widerstand des Potentionmeter
			R4.setOhm(i);
			
			//Netze erstellen
			Parallel R1R3 = new Parallel(R1, R3);
			Serial R1R2R3 = new Serial(R1R3, R2);
			Serial R4R5 = new Serial(R4, R5);
			Parallel R4R5R6 = new Parallel(R4R5, R6);
			Parallel R1R2R3R4R5R6 = new Parallel(R1R2R3, R4R5R6);

			//Ausgabe des Gesamt netzes
			System.out.println(R1R2R3R4R5R6.show());
			System.out.println("Potentiometer Widerstand: " + i + ": " + R1R2R3R4R5R6.ohm());
		}

	}
	
}
