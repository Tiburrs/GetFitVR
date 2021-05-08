using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

// creating data that can be serialized and saved
[System.Serializable]
public class PlayerData
{
  
  static protected int num_exercises = 3;  // number of exercises used
  static protected int old_date = 3;      // how many months the save file will keep
  
  [System.Serializable]
  // a class used to track the sets and reps completed on a given day
  public class Date{
    
    public string date;       // string containing data "month/day/year"
    public int[] workout;     // int array containing all reps. each element is a different set
    public float calories;
    
    // constructor function
    // input a string for current date and int array for reps
    public Date(string complete_date, int[] new_reps, float cal){
      date = complete_date;
      workout = new_reps;
      calories = cal;
    }
    
  }// end of Date class
  
  [System.Serializable]
  // a class used to keep track of each exercise
  public class Exercise{
    
    public List<Date> dates = new List<Date>();   // creating list of Date classes to append information on the fly
    public string exercise_name;                  // name of exercise
    private string date_info = "";                // string used to build exisiting date information
    
    // constructor function
    // takes string as input to set exercise_name
    public Exercise(string name){
      exercise_name = name;
    }
    
    // if nothing is given, name is set to a blank string
    public Exercise(){
      exercise_name = "";
    }
    
    // add a date class to your list
    // takes current date and int array
    public void generate_date(string complete_date, int[] reps, float cal){
      dates.Add(new Date(complete_date, reps, cal));
    }
    
    // functiont that returns all date information as a string
    // each set is seperated by a new line
    // ex: "1/1/2021\tSet:\t1\tReps:\t1\n"
    // date for each set is at the start of the line
    public string get_dates_info(){
      
      // clear current date_info
      date_info = "";
      
      // loop through each Date in list
      foreach(Date date_cur in dates){
        
        // looping through int array
        for(int i=0; i<date_cur.workout.Length;i++){
          date_info += date_cur.date + "\t#:\t" + (i+1) + "\tReps:\t" + date_cur.workout[i];
        
          if (i == date_cur.workout.Length - 1){
            date_info += "\tCals:\t" + date_cur.calories;
          }
          
          date_info += "\n";
        }
      }
      
      return date_info;
    }
    
  }// end of Exercise class
  
  // creating variables the player will use
  public string name;
  
  public Exercise exercise1 = new Exercise();
  public Exercise exercise2 = new Exercise();
  public Exercise exercise3 = new Exercise();
  
  // constructor to build player data
  public PlayerData ()
  {
    name = Player.read_name();
    exercise1 = Player.read_exercise(0);
    exercise2 = Player.read_exercise(1);
    exercise3 = Player.read_exercise(2);
    
  }
  
  // preparing variables do remove old dates
  static protected Exercise[] exercise_array;   // an array to go through the exercises
  static protected string[] today_array = System.DateTime.Now.ToString("MM/dd/yyyy").Split('/');
  static protected string dates_date;           // string to store date of exercise
  static protected string[] dates_array;        // string array to store month/day/year
  static protected int cut_month;               // int to hold cut off month as a number
  static protected int cut_day;                 // int to hold cut off day as a number
  static protected int cut_year;                // int to hold cut off year as a number
  static protected int date_month;              // int to hold date month as a number
  static protected int date_day;                // int to hold date day as a number
  static protected int date_year;               // int to hold date year as a number
  static protected bool need_deleted = false;   // boolean used to see if date is within range
  
  // function that will remove the any file older then the variable old_date
  public static PlayerData cleanFile(PlayerData info){
    
    // creating array to loop through
    exercise_array = new Exercise[] {info.exercise1, info.exercise2, info.exercise3};
    
    // getting todays date in numbers
    System.Int32.TryParse(today_array[0], out cut_month);
    System.Int32.TryParse(today_array[1], out cut_day);
    System.Int32.TryParse(today_array[2], out cut_year);
    
    // if the number of months go below zero, we need to add 12
    if (cut_month - old_date < 1){
      cut_month += 12;
    }
    
    // final cut off month
    cut_month -= old_date;
    
    // looping through each exercise
    for (int i = 0; i < num_exercises; i++){
      
      // looping through all the dates in that exercise
      for (int k = 0; k < exercise_array[i].dates.Count; k++){
        
        // getting the date of the first entry
        dates_date = exercise_array[i].dates[k].date;
        dates_array = dates_date.Split('/');
        
        // checking to see if that date is before or after the cutoff
        if(System.Int32.TryParse(dates_array[2], out date_year)){
          if(date_year == cut_year){
            System.Int32.TryParse(dates_array[0], out date_month);
            if(date_month == cut_month){
              System.Int32.TryParse(dates_array[1], out date_day);
              if (date_day < cut_day){
                need_deleted = true;
              }else{
                need_deleted = false;
              }
            }else if( date_month < cut_month){
              need_deleted = true;
            }else{
              need_deleted = false;
            }
          }else if(date_year < cut_year){
            need_deleted = true;
          }else{
            need_deleted = false;
          }
        }
        
        // if needed to delete, remove that element
        if(need_deleted){
          exercise_array[i].dates.Remove(exercise_array[i].dates[k]);
          k-=1;           // element was removed, need to redo the same index value
        }
      }// end of date loop
    }// end of exercise loop
    
    // adding updated versions back to the data
    info.exercise1 = exercise_array[0];
    info.exercise2 = exercise_array[1];
    info.exercise3 = exercise_array[2];
    
    return info;
  }// end of function cleanFile()
}// end of class
