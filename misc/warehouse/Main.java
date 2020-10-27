package penfactory;

import java.awt.EventQueue;
import javax.swing.JFrame;
import javax.swing.JTable;
import javax.swing.table.DefaultTableModel;
import javax.swing.JScrollPane;
import java.awt.Font;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JTextField;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.List;
import java.awt.event.ActionEvent;
import javax.swing.RowFilter;
import javax.swing.table.TableModel;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.table.TableRowSorter;
import javax.swing.text.NumberFormatter;
import javax.swing.JFormattedTextField;
import javax.swing.JSpinner;
import javax.swing.JSpinner.DefaultEditor;


/**
 * Starts the Warehouse Management software
 */
public class Main {

	/**
	 * Starts the Warehouse Management software
	 * 
	 * @param args The command line arguments
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					//create the GUI window
					GUI window = new GUI();
					
					//try to load the database at startup
					try {
						IO.Load(window.tableModel, Defaults.autoSaveLocation);
					}
					//If nothing was saved --> save
					catch(Exception ex){
						IO.Save(window.tableModel, Defaults.autoSaveLocation);
					}
					
					//and set it visible
					window.frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}
}





