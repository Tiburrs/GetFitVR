using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkFile : MonoBehaviour
{
    public void addSet(string date){
      
      //Player.add_date(0, "04/27/2021", new int[] {1,2,3});
      //Player.add_date(num, date, reps);
      Player.add_date(0, date, new int[] {1,2,3}, 40);
    }
}
