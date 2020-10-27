/*
 * 
 * 
 */

public class Resistor extends Net {
  
  
  // Anfang Attribute
  private final double widerstand;
  private String ID;
  // Ende Attribute
  
  //Anfang Konstruktoren
  public Resistor(double widerstand, String ID)
  {
	//füge die id des resistors allen ID's des Netzes hinzu
	super(ID);
	//initialisiere die attribute
    this.widerstand = widerstand;
    this.gesamtWiderstand = this.widerstand;
    this.ID = ID;
  }
  //Ende Konstruktoren

  // Anfang Methoden
  //gibt den Gesamtwiderstand des Netzes zurück
  public double ohm()
  {
	  return this.widerstand;
  }
  // Ende Methoden
} 

