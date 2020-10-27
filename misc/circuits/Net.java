
import java.util.TreeSet;

public abstract class Net{

	//Anfang Attribute
	//enth�lt ID's alle Resistoren im Netz
	protected java.util.TreeSet<String> resistors = new TreeSet<String>();
	protected double gesamtWiderstand = 0;
	//Ende Attribute
	
	//Anfang Konstruktoren
	public Net(String ID)
	{
		resistors.add(ID);
	}
	
	public Net(Net W1,Net W2) throws IDNotUniqueException
	{
		//leeres temp. TreeSet
		TreeSet<String> tmp = new TreeSet<String>();
				
		//alle Elemente in ein temp. TreeSet speichern
		//--> doppelte ID's gehen verloren
		tmp.addAll(W1.resistors);
		tmp.addAll(W2.resistors);
				
		try
		{
			//pr�ft ob das temp. TreeSet genauso viele Elemente hat wie das Netz Resistors 
			if((tmp.size()) == (W1.resistors.size() + W2.resistors.size()))
				this.resistors.addAll(tmp);
			
			//wenn nicht --> raise IDNotUniqueException
			else 
			{
				//alle Resistoren die in beiden Netzen vorkommen
				String duplikate = "";
				
				//schleifen um alle doppelten ID's zu finden
				for(String i : W1.resistors)
				{
					for(String j : W2.resistors)
					{
						if(i == j)
						{
							duplikate += i + " ";
						}
					}
				}
				//exception mit allen dopplente ID's
				throw new IDNotUniqueException(duplikate);
			}
				
		}
		//catch the exception
		catch(IDNotUniqueException e)
		{
			e.printStackTrace();
		}

	}
	//Ende Konstruktoren
	
	
	//Anfang Methoden
	
	//Gibt alle ID's in dem Netz aus
	public String show() {
		//String mit allen Resistor ID's in einem Netz
		String set = "";
		//Schleife �ber alle ID's in einem Netz
		for (String Resistor : resistors)
		{
			//f�ge alle Resitor id's an "set" an
			if(Resistor == this.resistors.last())
				set += Resistor;
			else
				set += Resistor + ", ";
		}
		//gibt "set" zur�ck
		return set;
	}
	
	//gibt den Gesamtwiderstand des Netzes zur�ck
	abstract double ohm();
	
	//Ende Methoden
}
