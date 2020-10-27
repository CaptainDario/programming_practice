package penfactory;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.Iterator;
import java.util.Vector;

import javax.swing.table.DefaultTableModel;




/**
 * Handles saving and loading the database to and from a file
 */
public class IO {

	

	/**
	 * Saves the current database to the file specified by path variable
	 * 
	 * @param tableModel The DefaultTableModel which will be saved.
	 * @param path The path to the file where the Data will be saved.
	 */
	public static void Save(DefaultTableModel tableModel, String path) {
		
		//creates file from the path where the database should be saved
		File file = new File(path);
		
		//try writing it to file
		try {
            ObjectOutputStream out = new ObjectOutputStream(new FileOutputStream(file));
            out.writeObject(tableModel.getDataVector());
            out.close();
		}
		//catch exceptions
        catch (Exception ex) {
            ex.printStackTrace();
        }
	}
	
	/**
	 * Loads a saved Database and writes it to the current Database.
	 * 
	 * @param tableModel The DefaultTableModel which saves the loaded data.
	 * @param path The Path to the file which will be loaded.
	 */
	public static void Load(DefaultTableModel tableModel, String path) {
		//creates file from the path where the database should be saved
		File file = new File(path);
		
		//remove all old rows from the table
		tableModel.setRowCount(0);
		
		//try reading the data from the file
		try {
            ObjectInputStream in = new ObjectInputStream(new FileInputStream(file));
            Vector rowData = (Vector)in.readObject();
            Iterator itr = rowData.iterator();
            //iterate over the loaded Vectors and add them to the table
            while(itr.hasNext()) {
                tableModel.addRow((Vector) itr.next());
            }
            in.close();
        }
		//catch exceptions
        catch (Exception ex) {
            ex.printStackTrace();
        }

	}
}
