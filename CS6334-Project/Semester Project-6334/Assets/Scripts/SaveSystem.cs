using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// creating static class to save/load information
public static class SaveSystem
{
  // function used to save player info
  public static void SavePlayer()
  {
    BinaryFormatter formatter = new BinaryFormatter();            //creating binary formatter
    string path = Application.persistentDataPath + "/player.sav"; //choosing a vaild path to save
    FileStream stream = new FileStream(path, FileMode.Create);    //open file stream

    PlayerData data = new PlayerData();                     // putting current information into data

    formatter.Serialize(stream, data);                            // saving file
    stream.Close();                                               // close stream
  }
  
  // function to load player
  public static PlayerData LoadPlayer()
  {
    
    string path = Application.persistentDataPath + "/player.sav";   // go to save location
    if (File.Exists(path))                                          // checking to see if file exist
    {
      BinaryFormatter formatter = new BinaryFormatter();            // creating formater
      FileStream stream = new FileStream(path, FileMode.Open);      // open stream
      
      PlayerData data = formatter.Deserialize(stream) as PlayerData;// loading information
      stream.Close();
      
      // only keeping data that is 3 months old
      //data = PlayerData.cleanFile(data);
      
      return data;
    }else{
      Debug.LogError("Save file not found in " + path);
      return null;
    }
  }
}
