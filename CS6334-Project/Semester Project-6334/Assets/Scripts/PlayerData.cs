using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
  public string name;
  public string[] exercise;
  public int[] sets;
  public int[] reps;
  
  public PlayerData (Player player)
  {
    name = player.name;
    exercise = player.exercise;
    sets = player.sets;
    reps = player.reps;
  }
}
