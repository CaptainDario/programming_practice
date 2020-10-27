/*
 * 
 * 
 */

public class IDNotUniqueException extends RuntimeException
{
	// Anfang Attribute
	private static final long serialVersionUID = 1L;
	// Ende Attribute

	//Anfang Konstruktoren 
	//Throw Exception with message
	public IDNotUniqueException(String message)
	{ 
		super(message + "sind in beiden Netzen vorhanden"); 
	}
	//Ende Konstruktoren
	
}
