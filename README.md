DataSeriesPod (c-sharp-data-series-pod)
=======================================

A Simple C# Data Logging Library.

## DataSeriesPod
This class provides basic functionality for recording and processing data values. It is typically used for preprocessing of frequently arriving sensor data. 

## Key features

* Written fully in C#
* Clean API
* Lightweight (small memory footprint)
* No additional dependencies
* Multi-platform (Windows, MacOS) 

### Constructor

* `DataSeriesPod(string)`
  Creates a new instance of this class taking a designation string as the single argument.

### Methods

* `Update(double)`
  Records the passed value. By passing `double.NaN` it has no effect. 
  
* `Restart()`
  All data recorded so far is discarded to start over. Typically used after consuming the wanted characteristic values of the recording. `Designation` is the only property conserved.

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

* `Duration`
  Returns the duration in seconds between the `FirstValueDate` and the `MostRecentValueDate`.

* `Designation`
  Returns the designation string as provided during creation of the object.

### Notes

All properties are getters only. A `double.NaN` is returned for properties which are (yet) undefined.

Once instantiated, it is not possible to modify the object's designation. 
The string provided in the constructor is trimmed and if empty, a generic designation is used. 

The data set recorded during the object's life cycle is never accessible; moreover it is not even stored internally. Only the selected characteristic values are accessible through properties.

### Usage

The folowing code fragment of a simple program shows the use of this class.
Sensor data is queried in an infinite loop and after every 10 seconds the average 
value and the number of samples are written to the console. 
In a similar way one can use other properties, like `SampleSize` or 
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
                    Console.WriteLine("Sample size: {0}", dsp.SampleSize);
                    Console.WriteLine("Mean value: {0}", dsp.AverageValue);
                    Console.WriteLine(dsp); // ToString() is implemented
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

