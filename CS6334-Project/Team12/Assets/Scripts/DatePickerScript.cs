using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DatePickerScript : MonoBehaviour
{

    // creating public variables this script controls
    public TextMeshProUGUI text_month;    // text field for month
    public TextMeshProUGUI text_day;      // text field for day
    public TextMeshProUGUI text_year;     // text field for year
    public TextMeshProUGUI date_field;    // text field for date
    public GameObject datePickerPanel;    // panel for date picker
    
    // creating private variables to do the logic
    private int month;                    // int for month calculations
    private int day;                      // int for day calculations
    private int year;                     // int for year calculations
    private string[] current_date;        // string to split date field
    
    // method used to close panel
    private void closePanel()
    {
      if(datePickerPanel.activeSelf){
        datePickerPanel.SetActive(false);
      }
    }

    private void Awake()
    {
        todayButton();
        closePanel();
    }
    // method used to open panel
    private void openPanel()
    {   
      if(!datePickerPanel.activeSelf){
        datePickerPanel.SetActive(true);
      }
    }
   
    // method used to start date picker
    public void openDatePicker()
    {
      openPanel();
      
      // moving month/date/year into correct fields
      current_date = date_field.text.Split('/');
      text_month.text = current_date[0];
      text_day.text = current_date[1];
      text_year.text = current_date[2];
      
    }
    
    // method used to increase month by one
    public void monthUp()
    {
      // checking to see if month can be parsed
      if(System.Int32.TryParse(text_month.text, out month)){
        // if month is higher then 11, we need to go back to one
        if(month >= 12)
        {
          text_month.text = "1";
        }else{
            text_month.text = "" + (month + 1);
        }
        
      }
      else{
        text_month.text = "1";
      }
    }
    
    // mehtod used to decrease month by one
    public void monthDown()
    {
      // checking to see if month can be parsed
      if(System.Int32.TryParse(text_month.text, out month)){
        // if the month is at 1 change it to 12
        if(month <= 1)
        {
          text_month.text = "12";
        }else{
            text_month.text = "" + (month - 1);
        }
        
      }
      else{
        text_month.text = "1";
      }
    }
    
    // method used to move day up one count
    public void dayUp()
    {
      //checking to see that day can be parsed
      if(System.Int32.TryParse(text_day.text, out day)){
        // checking to see what is the next day for increasing the number
        if(day >= 28)
        {
          if(System.Int32.TryParse(text_month.text, out month))
          {
            // switch case for deciding the number of days in that month
            switch(month)
            {
              case 4:
              case 6:
              case 9:
              case 11:
                if(day >= 30)
                {
                  text_day.text = "1";
                }else{
                  text_day.text = "" + (day + 1);
                }
                break;
              // in Februrary, we need to check for leap year.
              case 2:
                if(System.Int32.TryParse(text_year.text, out year))
                {
                  if(year % 4 == 0)
                  {
                    if(year % 100 == 0)
                    {
                      if(year % 400 == 0){
                        if(day >= 29)
                        {
                          text_day.text = "1";
                        }else{
                          text_day.text = "" + (day + 1);
                        }
                      }else{
                        if(day >= 28)
                        {
                          text_day.text = "1";
                        }else{
                          text_day.text = "" + (day + 1);
                        }
                      }
                    }else{
                      if(day >= 29)
                        {
                          text_day.text = "1";
                        }else{
                          text_day.text = "" + (day + 1);
                        }
                    }
                  }else{
                    if(day >= 28)
                    {
                      text_day.text = "1";
                    }else{
                      text_day.text = "" + (day + 1);
                    }
                  }
                }
                break;
              default:
                if(day >= 31)
                {
                  text_day.text = "1";
                }else{
                  text_day.text = "" + (day + 1);
                }
                break;
            }
          // if month cannot be parsed
          }else{
            text_month.text = "1";
          }
        // if day is less then 28
        }else{
          text_day.text = "" + (day + 1);
        }
      }else{
        text_day.text = "1";
      }
    }
    
    // method to decrease day by one
    public void dayDown()
    {
      // check to see if day can be parsed
      if(System.Int32.TryParse(text_day.text, out day)){
        // if day is 1 or less, move it to the end of month day
        if(day <= 1)
        {
          // checking to see month is parseable.
          if(System.Int32.TryParse(text_month.text, out month))
          {
            // depending on month, the end day changes
            switch(month)
            {
              case 4:
              case 6:
              case 9:
              case 11:
                text_day.text = "30";
                break;
              //if february, we need to check if it is a leap year
              case 2:
                if(System.Int32.TryParse(text_year.text, out year))
                {
                  if(year % 4 == 0)
                  {
                    if(year % 100 == 0)
                    {
                      if(year % 400 == 0){
                        text_day.text = "29";
                      }else{
                        text_day.text = "28";
                      }
                    }else{
                      text_day.text = "29";
                    }
                  }else{
                    text_day.text = "28";
                  }
                }
                break;
              default:
                text_day.text = "31";
                break;
            }
          // if not parasable, make it 1
          }else{
            text_day.text = "1";
          }
        // if the day is not 1 or lower, just decrease it
        }else{
          text_day.text = "" + (day - 1);
        }
        
      }
      // if not parsable, make it one
      else{
        text_day.text = "1";
      }
    }
    
    // mothod to increase year by one
    public void yearUp()
    {
      if(System.Int32.TryParse(text_year.text, out year)){
        text_year.text = "" + (year + 1);
      }
      else{
        text_year.text = "1";
      }
    }
    
    // method to decrease year by one
    public void yearDown()
    {
      if(System.Int32.TryParse(text_year.text, out year)){
        text_year.text = "" + (year - 1);
      }
      else{
        text_year.text = "1";
      }
    }
    
    //method used to get today's date
    public void todayButton()
    {
      date_field.text = System.DateTime.Now.ToString("MM/dd/yyyy");
      
      //setting values in the fields
      current_date = date_field.text.Split('/');
      text_month.text = current_date[0];
      text_day.text = current_date[1];
      text_year.text = current_date[2];
    }
    
    //method used to select the date
    public void returnBack()
    {
      current_date[0] = text_month.text;
      current_date[1] = text_day.text;
      current_date[2] = text_year.text;
      
      date_field.text = current_date[0] + "/" + current_date[1] + "/" + current_date[2];
      closePanel();
    }
}
