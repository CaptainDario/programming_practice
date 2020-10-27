package penfactory;

import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JFormattedTextField;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JScrollPane;
import javax.swing.JSpinner;
import javax.swing.JTable;
import javax.swing.JTextField;
import javax.swing.RowFilter;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;
import javax.swing.table.TableRowSorter;
import javax.swing.text.NumberFormatter;


/**
 * Creates and displays the GUI for the Warehouse Management software.
 * Enables the user to search through all properties of the products in the database
 */
public class GUI {

	/**
	 * The connection to the database functionality
	 */
	Assortment assortment;
	
	/**
	 * The frame for the main-window
	 */
	JFrame frame;
	
	/**
	 * The table which displays the database to the user
	 */
	JTable tableDatabase;
	
	/**
	 * The database to store all products and their properties
	 */
	DefaultTableModel tableModel;
	
	/**
	 * Used for searching in the database
	 */
	TableRowSorter<TableModel> rowSorter;
	
	/**
	 * The scroll-panel in which the table is integrated
	 */
	JScrollPane scrollPaneDatabase;
	
	/**
	 * The text-field in which the search term 
	 */
	JTextField textFieldSearch;
	/**
	 * The text-field in which the path where the database should be stored should be entered
	 */
	JTextField textFieldSave;
	/**
	 * The text-field in which the path to the saved database should be entered
	 */
	JTextField textFieldLoad;
	/**
	 * The text-field in which the category to delete should be entered
	 */
	JTextField textFieldCategoryToDelete;
	
	/**
	 * The button to open the pop-up-window for adding new products
	 */
	JButton btnAddProduct;
	/**
	 * The button to save the current database to the file
	 * specified by the textFieldSave
	 */
	JButton btnSave;
	/**
	 * The button to load a saved database from the file
	 * specified by the textFieldLoad
	 */
	JButton btnLoad;
	/**
	 * The button to remove all selected products
	 */
	JButton btnRemoveProduct;
	/**
	 * The button to delete all products from a category 
	 */
	JButton btnRemoveAllProducts;
	/**
	 * The button to add or remove instances from one product
	 * The product is defined by the selection(only ONE selection is possible)
	 */
	JButton btnRemoveProductInstances;
	
	/**
	 * The spinner which shows how many products should be removed or added
	 * negative - amount will be removed
	 * positive - amount will be added 
	 */
	JSpinner spinnerRemoveProductInstances;
	
	/**
	 * The label which indicates that the search term should be entered 
	 * in the text-field nearby
	 */
	JLabel labelSearchFor;
	
	/**
	 * Displays the path where the automatic save file is located
	 */
	JLabel autoSaveLocation;
	

	
	
	/**
	 * Create the GUI frame and all of its components
	 */
	public GUI(){
		
		//initialize the assortment which stores all the data
		assortment = new Assortment();
		
		//create the window-frame
		frame = new JFrame("Warehouse Management");
		frame.setBounds(100, 100, 919, 712);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.getContentPane().setLayout(null);
		
		//create the scroll-panel in which the table will be stored
		scrollPaneDatabase = new JScrollPane();
		scrollPaneDatabase.setBounds(10, 1, 879, 402);
		frame.getContentPane().add(scrollPaneDatabase);
		//initialize the model for the table view
		tableModel = new DefaultTableModel() {
			//set the cells in the table to not editable
			@Override
			public boolean isCellEditable(int row, int column) {
				return false;
			}
		};
		//create the actual table
		tableDatabase = new JTable(tableModel);
		tableDatabase.setFont(new Font("Tahoma", Font.PLAIN, 18));
		//add column headers to the table
		for(int i = 0; i < 6; i++) {
			tableModel.addColumn(Defaults.productAttributes[i]);
		}
		//add the table to the scroll-panel
		scrollPaneDatabase.setViewportView(tableDatabase);
		
		//Label indicating that the search term should be entered
		labelSearchFor = new JLabel("Search for:");
		labelSearchFor.setFont(new Font("Tahoma", Font.PLAIN, 16));
		labelSearchFor.setBounds(10, 416, 112, 22);
		frame.getContentPane().add(labelSearchFor);	
		//create the text-field in which the search term should be entered
		textFieldSearch = new JTextField();
		textFieldSearch.setBounds(134, 416, 233, 22);
		frame.getContentPane().add(textFieldSearch);
		textFieldSearch.setColumns(10);
	
		//
		rowSorter = new TableRowSorter<>(tableDatabase.getModel());
		tableDatabase.setRowSorter(rowSorter);
		textFieldSearch.getDocument().addDocumentListener(new DocumentListener(){

			@Override
			public void changedUpdate(DocumentEvent e) {
				//Not needed but needs to be overridden
				throw new UnsupportedOperationException("Not supported yet.");
			}

			//update when new character is inserted
			@Override
			public void insertUpdate(DocumentEvent e) {
				//get the search term
                String text = textFieldSearch.getText();
                
                //check if search term is empty
                if (text.trim().length() == 0) {
                	//disable filter
                    rowSorter.setRowFilter(null);
                } else {
                	//add row filter which displays all rows which contain the search term
                    rowSorter.setRowFilter(RowFilter.regexFilter("(?i)" + text));
                }
			}

			//update when character was removed
			@Override
			public void removeUpdate(DocumentEvent e) {
				//get the search term
				String text = textFieldSearch.getText();
				
				//check if search term is empty
				if (text.trim().length() == 0) {
					//disable filter
                    rowSorter.setRowFilter(null);
                } else {
                	//add row filter which displays all rows which contain the search term
                    rowSorter.setRowFilter(RowFilter.regexFilter("(?i)" + text));
                }
			}
			
		});
		
		//create the button for adding products to the table
		btnAddProduct = new JButton("Add Product");
		btnAddProduct.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				assortment.addProduct(tableModel);
			}
		});
		btnAddProduct.setBounds(653, 416, 236, 25);
		//append it to the frame
		frame.getContentPane().add(btnAddProduct);
		
	
		//create the button for deleting selected products from the table
		btnRemoveProduct = new JButton("Remove Selected Products");
		btnRemoveProduct.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				assortment.removeSelectedProduct(frame, tableDatabase, tableModel);
			}
		});
		btnRemoveProduct.setBounds(653, 454, 236, 25);
		//append it to the frame
		frame.getContentPane().add(btnRemoveProduct);
		
		
		//create the button for loading a database
		btnLoad = new JButton("Load from");
		btnLoad.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
				IO.Load(tableModel, textFieldLoad.getText());
			}
		});
		btnLoad.setBounds(10, 609, 200, 25);
		//append it to the frame
		frame.getContentPane().add(btnLoad);
		
		
		//create the button for saving the database
		btnSave = new JButton("Save in");
		btnSave.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				IO.Save(tableModel, textFieldSave.getText());
			}
		});
		btnSave.setBounds(10, 571, 200, 25);
		//append it to the frame
		frame.getContentPane().add(btnSave);
		
		//The path where the database should be saved
		textFieldSave = new JTextField();
		textFieldSave.setBounds(247, 572, 463, 22);
		frame.getContentPane().add(textFieldSave);
		textFieldSave.setColumns(10);
		
		//The path where the database should be loaded from
		textFieldLoad = new JTextField();
		textFieldLoad.setBounds(247, 610, 463, 22);
		//append it to the frame
		frame.getContentPane().add(textFieldLoad);
		textFieldLoad.setColumns(10);
		
		//The button which increases or decreases the amount of the selected product
		btnRemoveProductInstances = new JButton("Remove/Add selected Instances");
		btnRemoveProductInstances.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				assortment.removeProductInstances(spinnerRemoveProductInstances, tableDatabase, tableModel, frame);
			}
		});
		btnRemoveProductInstances.setBounds(653, 492, 236, 25);
		//append it to the frame
		frame.getContentPane().add(btnRemoveProductInstances);
		
		
		//Create the spinner to select the amount for adding and removing product instances
		spinnerRemoveProductInstances = new JSpinner();
		JFormattedTextField txt = ((JSpinner.NumberEditor) spinnerRemoveProductInstances.getEditor()).getTextField();
		((NumberFormatter) txt.getFormatter()).setAllowsInvalid(false);
		spinnerRemoveProductInstances.setBounds(525, 493, 116, 22);
		//append it to the frame
		frame.getContentPane().add(spinnerRemoveProductInstances);
		
		
		//Create the text-field in which the category to delete should be entered
		textFieldCategoryToDelete = new JTextField();
		textFieldCategoryToDelete.setBounds(525, 537, 116, 22);
		//append it to the frame
		frame.getContentPane().add(textFieldCategoryToDelete);
		textFieldCategoryToDelete.setColumns(10);
		
		
		//Create the Button to delete a category
		btnRemoveAllProducts = new JButton("Delete Category");
		btnRemoveAllProducts.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				String categoryToRemove = textFieldCategoryToDelete.getText().toString();
				assortment.removeCategory(categoryToRemove, frame, tableDatabase, tableModel);
			}
		});
		btnRemoveAllProducts.setBounds(653, 530, 236, 25);
		//append it to the frame
		frame.getContentPane().add(btnRemoveAllProducts);
		
		
		//Create a Label which displays the auto save location
		autoSaveLocation = new JLabel("Automatic saved files path: " + Defaults.autoSaveLocation);
		autoSaveLocation.setBounds(10, 640, 463, 22);
		//append it to the frame
		frame.getContentPane().add(autoSaveLocation);
		
	}
	
}
