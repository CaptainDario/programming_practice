/*
 * 
 * 
 */

public class Parallel extends Net{
	
	// Anfang Attribute
	//die beiden Nezte aus dem das Parallel Netz besteht
	private Net N1;
	private Net N2;
	// Ende Attribute

	//Anfang Konstruktoren
	Parallel(Net N1,Net N2) {
		super(N1, N2);
		this.N1 = N1;
		this.N2 = N2;
		this.gesamtWiderstand = 1/((1/N1.gesamtWiderstand) + (1/N2.gesamtWiderstand));
	}
	//Ende Konstruktoren
	
	// Anfang Methoden
	public double ohm()
	{
		return this.gesamtWiderstand;
	}
	// Ende Methoden
}

