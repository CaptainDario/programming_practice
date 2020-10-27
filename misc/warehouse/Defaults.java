package penfactory;

import java.nio.file.Path;
import java.nio.file.Paths;


/**
 * Stores default values for the Warehouse Management 
 */
public class Defaults {

	/**
	 * The attributes for every product
	 */
	public static String[] productAttributes = {"Name", "Weight (g)", "Place", 
			  "Category", "Price (Cent)", "Amount"};
	
	/**
	 * The standard path where the database will be automatically saved
	 */
	public static String autoSaveLocation = Paths.get(System.getProperty("user.home"), "PenfactoryData.txt").toString();
}
