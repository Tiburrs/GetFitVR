using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

  // setting put player information that is saved
  // making static so it can persist
  static protected string name = "Player";                //players name

  // initializing exercise classes for the player
  // making static so it can persist
  static protected PlayerData.Exercise exercise1 = new PlayerData.Exercise("sit-up");
  static protected PlayerData.Exercise exercise2 = new PlayerData.Exercise("twist-crunches");
  static protected PlayerData.Exercise exercise3 = new PlayerData.Exercise("twist-lung");
  static protected PlayerData.Exercise exercise4 = new PlayerData.Exercise("trash");

  // setting default data for testing
  void Start(){
    // this is how you create new data into class
//    exercise1.generate_date("1/2/2021", new int[] {8,20});
//    exercise1.generate_date("1/3/2021", new int[] {12});
//    exercise1.generate_date("1/10/2021", new int[] {14});
//    exercise2.generate_date("1/4/2021", new int[] {7});
//    exercise2.generate_date("1/5/2021", new int[] {9,11});
//    exercise2.generate_date("1/10/2021", new int[] {13,15});
//    exercise3.generate_date("1/7/2021", new int[] {3});
//    exercise3.generate_date("1/8/2021", new int[] {1});
//    exercise3.generate_date("1/10/2021", new int[] {5,6});
//    exercise3.generate_date("1/11/2021", new int[] {7,8});
//    exercise3.generate_date("1/12/2021", new int[] {9,10});
//    exercise3.generate_date("1/13/2021", new int[] {11,12});
  }
  
  // method used to read name of player
  static public string read_name()
  {
    return name;
  }
  // method used to read exercise class
  // will need an int to reference an exercise to read as an argument
  static public PlayerData.Exercise read_exercise(int exercise)
  {
    // switch case to find the exercise to return
    switch(exercise)
    {
      case 0:
        return exercise1;
        break;
      case 1:
        return exercise2;
        break;
      case 2:
        return exercise3;
        break;
      default:
        Debug.LogError("invalid exercise to read");
        return exercise4;
        break;
    }
  }
  // static method to add to othe exercise.
  // will need to pass in an int to reference with exercise to change, the date as a string
  // and the reps as an int array
  static public void add_date(int exercise, string info, int[] reps_add)
  {
    // switch case to find the exercise to change
    switch(exercise)
    {
      case 0:
        exercise1.generate_date(info, reps_add);
        break;
      case 1:
        exercise2.generate_date(info, reps_add);
        break;
      case 2:
        exercise3.generate_date(info, reps_add);
        break;
      default:
        Debug.LogError("invalid exercise to change");
        break;
    }
  }
  
  // function used save player information
  public void SavePlayer()
  {
    SaveSystem.SavePlayer();
  }
  
  //funciton used to load player information
  public void LoadPlayer()
  {
    // pulling saved info
    PlayerData data = SaveSystem.LoadPlayer();
    
    // updating player info
    name = data.name;
    exercise1 = data.exercise1;
    exercise2 = data.exercise2;
    exercise3 = data.exercise3;
  }
}
