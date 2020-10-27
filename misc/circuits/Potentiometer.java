/*
 * 
 * 
 */

public class Potentiometer extends Net{
	  
	  // Anfang Attribute
	  private String ID;
	  private double widerstand;
	  // Ende Attribute
	  
	  //Anfang Konstruktoren
	  public Potentiometer(String ID)
	  {
		super(ID);
	    this.widerstand = 0.0f;
	    this.ID = ID;
	    
	    this.gesamtWiderstand = widerstand;
	  }
	  //Ende Konstruktoren

	  // Anfang Methoden
	  public double ohm()
	  {
		  return this.gesamtWiderstand;
	  }

	  public void setOhm(double ohm)
	  {
		  this.gesamtWiderstand = ohm;
	  }
	  
	  public String getID()
	  {
	    return this.ID;
	  }
	  //Ende Methoden
	  
	
}
