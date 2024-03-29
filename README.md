DataSeriesPod
=============

A Simple C# Data Logging Library.

## DataSeriesPod
This class provides basic functionality for recording and processing data values. It is typically used for preprocessing and data reduction of frequently arriving sensor data. 

## Key features

* Written fully in C#
* Clean API
* Lightweight (small memory footprint)
* No additional dependencies
* Multi-platform (Windows, MacOS, Linux) 

### Constructors

* `DataSeriesPod(string)`
  Creates a new instance of this class taking a name string as the single argument.

* `DataSeriesPod()`
  Creates a new instance of this class using a GUID string as `Name`.

### Methods

* `Update(double)`
  Records the passed value. By passing `double.NaN` the call is without effect. 
  
* `Restart()`
  All data recorded so far is discarded to start over. Typically used after consuming the wanted characteristic values of the recording. `Name` is the only property conserved.

### Properties

* `SampleSize`
  Returns the number of samples recorded since the last `Restart()`.

* `AverageValue`
  Returns the arithmetic mean of all values recorded.

* `MinimumValue`
  Returns the smallest value recorded.

* `MaximumValue`
  Returns the largest value recorded.

* `Range`
  Returns the difference between the smallest and largest value recorded.

* `CentralValue`
  Returns the mid-range in the data set. This is the arithmetic mean of the maximum and minimum of all recorded values.

* `FirstValue`
  Returns the first value recorded following a `Restart()`.

* `MostRecentValue`
  Returns the most recent value recorded.

* `FirstValueDate`
  Returns the date when the first value was recorded.

* `MostRecentValueDate`
  Returns the date when the most recent value was recorded.

* `MinimumValueDate`
  Returns the date when the smallest value was recorded.

* `MaximumValueDate`
  Returns the date when the largest value was recorded.

* `Duration`
  Returns the duration in seconds between the `FirstValueDate` and the `MostRecentValueDate`.

* `Name`
  Returns the name string as provided during creation of the object.

### Notes

All properties are getters only. A `double.NaN` is returned for properties which are (yet) undefined.

Once instantiated, it is not possible to modify the object's name. 
The string provided in the constructor is trimmed and if empty, a GUID string is used. 

The data set recorded during the object's life cycle is never accessible; moreover it is not even stored internally. Only the selected characteristic values are accessible through properties.

The arithmetic mean is computed in a numerically stable way. For details see https://diego.assencio.com/?index=c34d06f4f4de2375658ed41f70177d59

### Usage

The following code fragment of a simple program shows the use of this class.
Here sensor data are queried in an infinite loop and after every 10 seconds the average 
value and the number of samples are written to the console. 
In a similar way one can leverage other properties, like `SampleSize` or 
`MostRecentValueDate`, to escape from the loop. 

```cs
using System;
using At.Matus.DataSeriesPod;

namespace dspTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var dsp = new DataSeriesPod("Sensor ID: 314");
            double samplingDuration = 10.0; // seconds
            while (true)
            {
                dsp.Update(GetSensorValue());
                if(dsp.Duration >= samplingDuration)
                {
                    Console.WriteLine($"Sample size: {dsp.SampleSize}");
                    Console.WriteLine($"Mean value: {dsp.AverageValue}");
                    Console.WriteLine(dsp); // ToString() is implemented also
                    dsp.Restart();
                }
            }
        }
        
        public static double GetSensorValue()
        {
            return 0.0; // here one should query an actual sensor
        }
    }
}
```
