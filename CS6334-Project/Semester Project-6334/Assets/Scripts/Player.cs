using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

  // setting put player information that is saved
  public string name = "John Doe";                //players name

  // initializing exercise classes for the player
  public PlayerData.Exercise exercise1 = new PlayerData.Exercise("sit-up");
  public PlayerData.Exercise exercise2 = new PlayerData.Exercise("twist-crunches");
  public PlayerData.Exercise exercise3 = new PlayerData.Exercise("twist-lung");

  // setting default data for testing
  void Start(){
    // this is how you create new data into class
    exercise1.generate_date("1/2/21", new int[] {8,20});
    exercise1.generate_date("1/3/21", new int[] {12});
    exercise1.generate_date("1/10/21", new int[] {14});
    exercise2.generate_date("1/4/21", new int[] {7});
    exercise2.generate_date("1/5/21", new int[] {9,11});
    exercise2.generate_date("1/10/21", new int[] {13,15});
    exercise3.generate_date("1/7/21", new int[] {3});
    exercise3.generate_date("1/8/21", new int[] {1});
    exercise3.generate_date("1/10/21", new int[] {5,6});
    exercise3.generate_date("1/11/21", new int[] {7,8});
    exercise3.generate_date("1/12/21", new int[] {9,10});
    exercise3.generate_date("1/13/21", new int[] {11,12});
  }
  
  // function used save player information
  public void SavePlayer()
  {
    SaveSystem.SavePlayer(this);
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
