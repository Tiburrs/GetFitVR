using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trash : MonoBehaviour
{
    public void goToRecordRoom()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
