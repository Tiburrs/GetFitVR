using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

// creating data that can be serialized and saved
[System.Serializable]
public class PlayerData
{
  // a class used to track the sets and reps completed on a given day
  public class Date{
    
    public string date;       // string containing data "month/day/year"
    public int[] workout;     // int array containing all reps. each element is a different set
    
    // constructor function
    // input a string for current date and int array for reps
    public Date(string complete_date, int[] new_reps){
      date = complete_date;
      workout = new_reps;
    }
    
  }// end of Date class
  
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
    public void generate_date(string complete_date, int[] reps){
      dates.Add(new Date(complete_date, reps));
    }
    
    // functiont that returns all date information as a string
    // each set is seperated by a new line
    // ex: "1/1/21\tSet:\t1\tReps:\t1\n"
    // date for each set is at the start of the line
    public string get_dates_info(){
      
      // clear current date_info
      date_info = "";
      
      // loop through each Date in list
      foreach(Date date_cur in dates){
        
        // looping through int array
        for(int i=0; i<date_cur.workout.Length;i++){
          date_info += date_cur.date + "\tSet:\t" + (i+1) + "\tReps:\t" + date_cur.workout[i] + "\n";
        }
      }
      
      return date_info;
    }
    
  }
  
  // creating variables the player will use
  public string name;
  
  public Exercise exercise1 = new Exercise();
  public Exercise exercise2 = new Exercise();
  public Exercise exercise3 = new Exercise();
  
  // constructor to build player data
  public PlayerData (Player player)
  {
    name = player.name;
    exercise1 = player.exercise1;
    exercise2 = player.exercise2;
    exercise3 = player.exercise3;
  }
}
