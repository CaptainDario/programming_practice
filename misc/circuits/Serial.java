/*
 * 
 * 
 */

public class Serial extends Net{
	
	// Anfang Attribute
	private Net N1;
	private Net N2;
	// Ende Attribute

	//Anfang Konstruktoren
	Serial(Net N1,Net N2) 
	{
		super(N1, N2);
		this.N1 = N1;
		this.N2 = N2;
		this.gesamtWiderstand = N1.gesamtWiderstand + N2.gesamtWiderstand;
	}
	//Ende Konstruktoren
	
	public double ohm() 
	{
		return this.gesamtWiderstand;
	}
	//Ende Methoden
}

