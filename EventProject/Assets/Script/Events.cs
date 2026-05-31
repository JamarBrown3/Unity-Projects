using UnityEngine;
using System;

[Serializable]
public struct Time
{
    public int Year;
    public int Month;
    public int Day;
    public int Hour;
    public int Minute;

    // making an Array of Time using a public constructor
    public Time(int year, int month, int day, int hour, int minute)
    {
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
    }
    // Override public string to make sure that the String and data is formatted to be a
    // string

    public override string ToString()
    {
        return $"{Year}/{Month}/{Day} {Hour:D2}:{Minute:D2}";
    }
}

[Serializable]
public class Events
{
    public string name;
    public Time time;
    public string eventType;
    public string eventDescription;
    public bool solved;

    // Example method to initialize the time array

}
