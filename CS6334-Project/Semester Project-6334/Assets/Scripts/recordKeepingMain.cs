using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class recordKeepingMain : MonoBehaviour
{
    // panel that will display the contents
    public GameObject panel2;
    
    // text fields that are used in the script
    public TextMeshProUGUI date_field;    // field that has the date
    public TextMeshProUGUI text_field_name;   // text field for player's name
    public TextMeshProUGUI text_field1;   // text field for button 1
    public TextMeshProUGUI text_field2;   // text field for button 2
    public TextMeshProUGUI text_field3;   // text field for button 3
    public TextMeshProUGUI text_field4;   // text field for title of printed display
    public TextMeshProUGUI text_field5;   // text field for contents to be printed (one lare field)
    
    private int current_pos=0;            //variable used to keep track where in the array to start for printing to panel
    private PlayerData.Exercise current_exercise; //variable to store current exercise class (changed with exercise selection)
    private string[] text5_array;         //used to parse total contents into individual lines
    private string[] date_array;          //used to parse date into month, day, and year
    private string[] info_date_array;     //used to parse date of each exercise into month, day, and year
    private string prev_date = "";             //variable used to filter out printing redundant dates
    private bool date_found = false;      //bool used to filter through information to get to selected date
    private int date_month;               //variable used to store selected date month as an int
    private int date_day;                 //variable used to store selected date day as an int
    private int date_year;                //variable used to store selected date year as an int
    private int info_month;               //variable used to store exercise date month as an int
    private int info_day;                 //variable used to store exercise date day as an int
    private int info_year;                //variable used to store exercise date year as an int

    // Start is called before the first frame update
    void Start()
    {
      // setting button names to show the correct exercise and showing players name
      text_field_name.text = Player.read_name();
      text_field1.text = Player.read_exercise(0).exercise_name;
      text_field2.text = Player.read_exercise(1).exercise_name;
      text_field3.text = Player.read_exercise(2).exercise_name;
    }

    // function used to turn the panel that shows the information active
    private void openPanel(){
      panel2.SetActive(true);
    }
    
    // function that is executed when first button is pressed
    public void button_exercise1(){
      // checking to see if panel is active
      if(!panel2.activeSelf){
        openPanel();
      }
      // setting global variables for navigation keys on panel2
      current_pos=0;
      current_exercise = Player.read_exercise(0);
      date_found = false;
      
      // setting the title for panel2
      text_field4.text = text_field1.text;
      
      // calling function to generate the exercise information the first time
      generateContents(current_exercise);
    }
    
    // function that is executed when second button is pressed
    public void button_exercise2(){
      // checking to see if panel is active
      if(!panel2.activeSelf){
        openPanel();
      }
      // setting global variables for navigation keys on panel2
      current_pos=0;
      current_exercise = Player.read_exercise(1);
      date_found = false;
      
      // setting the title for panel2
      text_field4.text = text_field2.text;
      
      // calling function to generate the exercise information the first time
      generateContents(current_exercise);
    }
    
    // function that is executed when third button is pressed
    public void button_exercise3(){
      // checking to see if pane2 is active
      if(!panel2.activeSelf){
        openPanel();
      }
      // setting global variables for navigation keys on panel2
      current_pos=0;
      current_exercise = Player.read_exercise(2);
      date_found = false;
      
      // setting the title for panel2
      text_field4.text = text_field3.text;
      
      // calling function to generate the exercise information the first time
      generateContents(current_exercise);
    }
    
    // function to generat the exercise information the first time.
    // pass in the exercise class to be printed
    private void generateContents(PlayerData.Exercise workout){
    
      // preparing information for processing
      text5_array = workout.get_dates_info().Split('\n');   // parcing data returned from Exercise class into lines
      date_array = date_field.text.Split('/');              // parcing date field into month, day, and year
      System.Int32.TryParse(date_array[0], out date_month); // converting month to int
      System.Int32.TryParse(date_array[1], out date_day);   // converting day to int
      System.Int32.TryParse(date_array[2], out date_year);  // converting year to int
      text_field5.text = "";                                // clearing current contents in field
      
      // looping through al the lines returned from the Exercise class
      // max number of lines printed will be 6
      for(int i=current_pos; i<text5_array.Length && i<(6+current_pos); i++){
        // checking to see if we are placed in the correct position for the selected date
        if(!date_found){
        
          info_date_array = text5_array[i].Split('\t');     //parcing line to get date info
          info_date_array = info_date_array[0].Split('/');  //parcing date into month, day, and year
          
          // a redunant check to prevent multiple fail to parse function
          if(System.Int32.TryParse(info_date_array[0], out info_month)){  //TryParse returns bool if function executed
            System.Int32.TryParse(info_date_array[2], out info_year);     //TryParse will save string into int variable after out
            
            // checking to see if the selected date is before or after the current position in the stored data
            if(date_year > info_year){              // if selected date is larger go to next entry
              current_pos++;
            }else if(date_year == info_year){       // if selected date is equal, we need to check month
              if(date_month > info_month){          // if selected month is larger, go to next entry
                current_pos++;
              }else if(date_month == info_month){   // if selected month is equal, we need to check days
                System.Int32.TryParse(info_date_array[1], out info_day);// parse day into int
                if(date_day > info_day){            // if selected day is larger, go to next entry
                  current_pos++;
                }else{                              // if selected day is equal or larger, we found where to start
                  date_found=true;
                  i--;                              // subtract one to restart at this line for printing
                }
              }else{                                // if month is smaller, we found our date
                date_found = true;
                i--;                                // subtract one to restart at this line for printing
              }
            }else{                                  // if year is less, we found our date
              date_found = true;
              i--;                                  // subtract one to restart at this line for printing
            }
          }else{                                    // if we cannot parse line, move to next position
            current_pos++;
          }
          
        // end of finding position to start
        }else{
        
          printContents();
          
        }
      }// end of for loop
      
      // if date_found is still false, set it to true and print last 6 lines
      if(!date_found){
        date_found = true;
        current_pos -= 7;
        
        // current_pos should not be negative
        if(current_pos < 0){
          current_pos = 0;
        }
        
        printContents();
      }
    }
    
    // function to print contents after position is set
    private void printContents(){
      
      // clearing text field
      text_field5.text = "";
      for(int i=current_pos; i<text5_array.Length && i<(6+current_pos); i++){
        
          // seperating each line to customize line format
          info_date_array = text5_array[i].Split('\t');
          
          // checking to see if previous line matches current line date
          if(info_date_array[0]==prev_date){
          
            // outputting line data (without date)
            text_field5.text += "\t\t";
            for(int j=1; j<info_date_array.Length;j++){
              text_field5.text += info_date_array[j] + "\t";
            }
            
          }else{
            
            prev_date = info_date_array[0];   //setting prev_date to current date
            // outputting line data
            for(int j=0; j<info_date_array.Length;j++){
              text_field5.text += info_date_array[j] + "\t";
            }
          }
          
          // adding new line for next line
          text_field5.text += "\n";
      }
    }
    
    // function used to move contents up
    public void moveContentsUp(){
      
      // checking to see if current position is showing last 6 lines
      if((text5_array.Length - (current_pos + 1)) > 6){
        current_pos++;                  // if lines are left, increase current_pos
      }
      
      // clear text field
      text_field5.text = "";
      
      printContents();
      
    }
    
    public void moveContentsDown(){
    
      // check to see if current position is the start
      if(current_pos > 0){
        current_pos--;      // if not at the start, decrease the position
      }
      
      printContents();

    }
}
