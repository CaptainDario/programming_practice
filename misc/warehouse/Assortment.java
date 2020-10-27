package penfactory;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.List;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JSpinner;
import javax.swing.JTable;
import javax.swing.JTextField;
import javax.swing.table.DefaultTableModel;




/**
 * Handles the database.
 * 
 * Makes it possible to add new products to the Database.
 * Makes it possible to remove products.
 * Makes it possible to increase or decrease the amount of existing products.
 * Removing all products from one category.
 * Checks for valid properties for new products.
 */
public class Assortment {
	
	/**
	 * The frame for the pop-up-window for adding new products to the database.
	 */
	private JFrame popupAddProduct;
	
	/**
	 * Sets the pop-up-window for adding products.
	 */
	public Assortment() {
		this.popupAddProduct = null;
	}
	

	/**
	 * Removes the currently selected product(s) from the Database.
	 * It is possible to remove multiple Entries at once when they are all selected.
	 * 
	 * @param frame The frame of the GUI window.
	 * @param tableDatabase The graphical table in which the Database is displayed.
	 * @param tableModel The Database in which all product entries were saved.
	 */
	void removeSelectedProduct(JFrame frame, JTable tableDatabase, DefaultTableModel tableModel) {
		
		//only delete a product if there is a row selected in the table
		if(tableDatabase.getSelectedRow() != -1) {
			
			//get all currently selected rows
			int[] selectedRows = tableDatabase.getSelectedRows();
			
			//iterate over the selected rows and delete them
			for(int i = selectedRows.length - 1; i >= 0 ;i--) {
				tableModel.removeRow(selectedRows[i]);
			}
			
			//Save the database
			IO.Save(tableModel, Defaults.autoSaveLocation);
		}
		//show a message to the user if no row is selected
		else {
			JOptionPane.showMessageDialog(frame, "No product selected!");
		}
	}
	

	/**
	 * Removes/adds instances from/to a product.
	 * If a negative number was entered this amount will be removed from the product's amount.
	 * If a positive number was entered this amount will be added to the product's amount.
	 * 
	 * @param spinnerRemoveProductInstances The GUI spinner which displays the
	 *        amount of products that will be added/removed.
	 * @param tableDatabase The graphical table in which the Database is displayed.
	 * @param tableModel The Database in which all product entries were saved.
	 * @param frame The frame of the GUI window.
	 */
	void removeProductInstances(JSpinner spinnerRemoveProductInstances, JTable tableDatabase,
								DefaultTableModel tableModel, JFrame frame) {
		
		//get String from the amount-spinner and convert it to Integer
		Integer amountToRemove = Integer.parseInt(spinnerRemoveProductInstances.getValue().toString());
		
		//only delete instances of a product if there is a row selected
		if(tableDatabase.getSelectedRow() != -1) {
			
			//calculate the new amount which will be written in the Database
			Integer selectedRow = tableDatabase.getSelectedRow();
			Integer valueInTable = Integer.parseInt(tableModel.getValueAt(selectedRow, 5).toString());
			Integer newValue = valueInTable + amountToRemove;
			
			//check if more products should be removed then there are at this place
			if(newValue < 0) {
				//show a dialog if there are not enough products at the selected place
				//Yes - Remove as much products as possible
				//No - quit
				Integer chosen = JOptionPane.showConfirmDialog(frame, "There are not enough products at this place!"
																+ "\nRemove as much as possible from this place?", "Warning",
																JOptionPane.YES_NO_OPTION);
				//yes was chosen
				if(chosen == JOptionPane.YES_OPTION) {
					tableModel.setValueAt(0, selectedRow, 5);
					spinnerRemoveProductInstances.setValue(newValue);
				}
			}
			
			//if there are enough products at this place
			if(newValue >= 0) {
				//set the new value
				tableModel.setValueAt(newValue, selectedRow, 5);
				spinnerRemoveProductInstances.setValue(0);
			}
			
			//check if there are no products left at this place
			if(newValue == 0 ||
				Integer.parseInt(tableModel.getValueAt(selectedRow, 5).toString()) == 0) {
							
				//ask if the entry with amount = 0 should be deleted
				//so that the place can be reused
				Integer chosen = JOptionPane.showConfirmDialog(frame, "There are no more products at this place. \nRemove entry from this place?",
														"Warning", JOptionPane.YES_NO_OPTION);
				
				//yes was chosen
				if(chosen == JOptionPane.YES_OPTION) {
					//remove the entry
					removeSelectedProduct(frame, tableDatabase, tableModel);
				}		
			}
			
			//automatically save if value has changed
			if(valueInTable != Integer.parseInt(tableModel.getValueAt(selectedRow, 5).toString())) {
				System.out.println("Saved");
				IO.Save(tableModel, Defaults.autoSaveLocation);
			}
		}
		//show a message to the user if no row was selected
		else {
			JOptionPane.showMessageDialog(frame, "No product selected!");
		}
	}
	
	
	/**
	 * Every entry which belongs to the category will be listed.
	 * Then the user is asked by the system if all data should be
	 * deleted. 
	 * 
	 * @param categoryToRemove The Category which should be removed.
	 * @param frame The frame of the GUI window.
	 * @param tableDatabase The graphical table in which the Database is displayed.
	 * @param tableModel The Database in which all product entries were saved.
	 */
	void removeCategory(String categoryToRemove, JFrame frame,
						JTable tableDatabase, DefaultTableModel tableModel) {
		
		//list with all the indices of all products belonging to the category
		List<Integer> indices = new ArrayList<Integer>();
		//all names of all products belonging to the category
		String names = "";
		
		//get all products which belong to this category
		for(int i = tableModel.getRowCount() - 1; i >= 0; i--) {

			//check if product belongs to category
			if(tableModel.getValueAt(i, 3).toString().equals(categoryToRemove)) {
				//collect the indices
				indices.add(i);
				//collect all names
				names += tableModel.getValueAt(i, 0).toString() + "\n";
			}
		}
		
		//if at least one product belonging to the category was found
		if(indices.size() > 0) {
			//Ask if the user is certain to delete all found products
			Integer chose = JOptionPane.showConfirmDialog(frame, "Following products will be deleted:\n"
														+ names, "Warning", JOptionPane.YES_NO_OPTION);
			
			//no - quit
			if(chose == JOptionPane.YES_OPTION) {
				//Remove all entries
				for(int i = 0; i < indices.size(); i++) {
					tableModel.removeRow(indices.get(i));
				}
				
				//save the database
				IO.Save(tableModel, Defaults.autoSaveLocation);
			}

		}
		
		
			
		//No products belongs to this category
		//display a message
		if(indices.size() == 0) {
			JOptionPane.showMessageDialog(frame, "Could not find any product of this category!", "Error", JOptionPane.YES_NO_OPTION);
		}
		
		
	}
	
	
	/**
	 * Opens a window and asks for the product attributes.
	 * The attributes will be checked for following constraints:
	 * 		name             - can not be empty
	 *		weight (in gram) - must be a positive integer
	 *		place            - must be an integer
	 *		category         - can not be empty
	 *      price (in cents) - must be a positive integer greater 0
	 *      amount           - must be a positive integer
	 * If one attribute is not correct a message to the user will be shown
	 * explaining the errors.
	 *      
	 * @param tableModel The Database in which all product entries were saved.
	 */
	void addProduct(DefaultTableModel tableModel) {

		//check if the pop-up window for adding new products is already opened
		if(popupAddProduct != null) {
			//display Message that it is already opened
			JOptionPane.showMessageDialog(popupAddProduct, "Add product Dialog already opened!");
			//and do NOT open a new one
			return;
		}
		
		//Create the pop-up-frame, set size and visibility
		popupAddProduct = new JFrame("Add Product"); 
		popupAddProduct.setSize(375, 375);
		popupAddProduct.setVisible(true);
		
		//if the add-product-dialog was closed
		popupAddProduct.addWindowListener(new java.awt.event.WindowAdapter() {
		    @Override
		    public void windowClosing(java.awt.event.WindowEvent windowEvent) {
		        //set the pop-up frame to null
		    	popupAddProduct = null;
		    }
		});
		
		//Create the input-text-fields and the labels
		JTextField[] textFields = new JTextField[6];
		for(int i = 0; i < 6; i++) {
			//LABELS
			JLabel label = new JLabel(Defaults.productAttributes[i]);
			label.setBounds(10, 10 + i * 50, 100, 20);
			popupAddProduct.getContentPane().add(label); 
			
			//TEXTFIELDS
			textFields[i] = new JTextField("");
			textFields[i].setBounds(150, 10 + i * 50, 200, 20);
			popupAddProduct.getContentPane().add(textFields[i]); 
		}
		

		//Create the add-button and set size
		JButton buttonAdd = new JButton("Add");
		buttonAdd.setBounds(250, 300, 100, 25);
		//Add the event which is executed when the button is pressed
		buttonAdd.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				String errors = "";
				//check for valid name
				if(textFields[0].getText().equals("")) {
					errors += "No valid name was entered! \n";
				}
				//check for valid weight
				if(textFields[1].getText().equals("") ||
					checkValidWeight(textFields[1].getText()) == false) {
					errors += "No valid weight was entered! \n";
				}
				//check for valid place
				String eval = checkValidPlace(textFields, tableModel);
				if(textFields[2].getText().equals("") || !eval.equals("true")) {
					errors += eval;
				}
				//check for valid category
				if(textFields[3].getText().equals("")) {
					errors += "No valid category was entered! \n";
				}
				//check for valid price
				if(textFields[4].getText().equals("") ||
					checkValidPrice(textFields[4].getText()) == false) {
					errors += "No valid price was entered! \n";
				}
				//check for valid amount
				if(textFields[5].getText().equals("") ||
					checkValidAmount(textFields[5].getText()) == false) {
					errors += "No valid amount was entered!";
				}
				//output all collected errors if there are any
				if(errors != "") {
					JOptionPane.showMessageDialog(popupAddProduct, errors);
				}
				//otherwise add the new Product to the Database
				else {
					tableModel.addRow(new Object[] {textFields[0].getText(), textFields[1].getText(),
													textFields[2].getText(), textFields[3].getText(),
													textFields[4].getText(), textFields[5].getText()});
					
					//and save the database
					IO.Save(tableModel, Defaults.autoSaveLocation);
				}
			}
		});
		//Add the add-button to the pop-up-window
		popupAddProduct.getContentPane().add(buttonAdd);
		
		//aid-label
		JLabel labelInvisible = new JLabel("");
		labelInvisible.setBounds(10, 250, 100, 100);
		popupAddProduct.getContentPane().add(labelInvisible);
	}
	
	/**
	 * Check if the entered weight is valid(integer greater than zero).
	 * 
	 * @param str The weight which should be checked.
	 * @return true  - if the entered weight is valid.
	 *         false - if the entered weight is not valid.
	 */
	public boolean checkValidWeight(String str) {  
		try {  
			//try parsing the string to an integer
			Integer integer = Integer.parseInt(str);
			//check if the integer is greater then zero
			if(integer < 1) {
				return false;
			}
		}  
		//could not parse to integer
		catch(NumberFormatException nfe) {  
			return false;  
		}  
		//it is a valid weight
		return true;  
	}
	

	/**
	 * Checks if the entered place is valid(integer and place is not taken).
	 * 
	 * @param textFields All text-fields of the pop-up-window
	 * @param tableModel The Database in which all product entries were saved.
	 * @return "Merged with same article! \n"   - if the same article is located at this place.
	 * 	       "Place already taken! \n"        - if a different article is located at this place.
	 *         "No valid place was entered! \n" - if the entered string is not an integer.
	 *         "true"                           - if the input is valid.
	 */
	public String checkValidPlace(JTextField[] textFields, DefaultTableModel tableModel) {  
		  try {  
			//get the text from the text-field
			String place = textFields[2].getText();
			//try to parse integer
		    Integer integer = Integer.parseInt(place);
		    
		    if(tableModel.getRowCount() > 0) {
		    	for(int i = 0; i < tableModel.getRowCount(); i++) {
		    		System.out.println(tableModel.getValueAt(i, 2));
		    		
		    		//check if place is already taken
			    	if(place.equals(tableModel.getValueAt(i, 2))) {
			    		//check if the same product is at the place
			    		if(tableModel.getValueAt(i, 0).equals(textFields[0].getText()) &&
		    			tableModel.getValueAt(i, 1).equals(textFields[1].getText()) &&
		    			tableModel.getValueAt(i, 3).equals(textFields[3].getText()) &&
		    			tableModel.getValueAt(i, 4).equals(textFields[4].getText())) {
			    			
			    			//Add the amount of both products together
			    			Integer oldValue = Integer.parseInt(tableModel.getValueAt(i, 5).toString());
			    			Integer addition = Integer.parseInt(textFields[5].getText());
			    			tableModel.setValueAt(oldValue + addition, i, 5);
			    			
			    			return "Merged with same article! \n";
			    		}
			    		//a different product is located at this place
			    		else {
			    			return "Place already taken! \n";
			    		}
			    	}
			    }
		    }   
		  }  
		  //could not parse string to integer
		  catch(NumberFormatException nfe) {  
		    return "No valid place was entered! \n";  
		  }  
		  //product can be added to this place
		  return "true";  
	}
	

	/**
	 * Check if the entered price is valid(integer greater than zero).
	 * 
	 * @param str The price which should be checked.
	 * @return true  - if the entered price is valid.
	 *         false - if the entered price is not valid.
	 */
	public boolean checkValidPrice(String str) {  
		try {
			//try parsing the string to an integer
			Integer integer = Integer.parseInt(str);
			//check if the integer is greater then zero
			if(integer < 1) {
				return false;
			}
		}  
		//could not parse to integer
		catch(NumberFormatException nfe) {  
			return false;  
		}  
		//it is a valid price
		return true;  
}

	
	/**
	 * Check if the entered amount is valid(integer greater than zero).
	 * 
	 * @param str The amount which should be checked.
	 * @return true  - if the entered amount is valid.
	 *         false - if the entered amount is not valid.
	 */
	public boolean checkValidAmount(String str) {  
		try {  
			//try parsing the string to an integer
			Integer integer = Integer.parseInt(str);
			//check if the integer is not negative
			if(integer < 0) {
				return false;
			}
		}  
		//could not parse to integer
		catch(NumberFormatException nfe) {  
			return false;  
		}
		//it is a valid amount
		return true;  
}
	
}
