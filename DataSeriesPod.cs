using System;

namespace At.Matus.DataSeriesPod
{
    public class DataSeriesPod
    {

        #region Ctor

        public DataSeriesPod(string designation)
        {
            Designation = designation.Trim();
            if (string.IsNullOrEmpty(Designation))
                Designation = noDesignationSpecified;
            Restart();
        }

        public DataSeriesPod() : this("") { }

        #endregion

        #region Properties

        public double AverageValue { get { return GetValueIfValid(sumValue / SampleSize); } }
        public double Range { get { return GetValueIfValid((maximumValue - minimumValue)); } }
        public double FirstValue { get; private set; }
        public double MostRecentValue { get; private set; }
        public double MaximumValue { get { return GetValueIfValid(maximumValue); } }
        public double MinimumValue { get { return GetValueIfValid(minimumValue); } }
        public double CentralValue { get { return GetValueIfValid((maximumValue + minimumValue) / 2.0); } }
        public long SampleSize { get; private set; }
        public string Designation { get; private set; }
        public DateTime FirstDate { get; private set; }
        public DateTime MostRecentValueDate { get; private set; }
        public double Duration { get { return GetDurationInSeconds(); } }

        #endregion

        #region Methods

        public void Restart()
        {
            SampleSize = 0;
            sumValue = 0;
            maximumValue = double.MinValue;
            minimumValue = double.MaxValue;
            FirstValue = double.NaN;
            MostRecentValue = double.NaN;
            FirstDate = DateTime.UtcNow;
            MostRecentValueDate = DateTime.UtcNow;
        }

        public void Update(double value)
        {
            if (double.IsNaN(value)) return;
            if (SampleSize >= long.MaxValue - 1) return;
            SampleSize++;
            if (SampleSize == 1)
            {
                FirstValue = value;
                FirstDate = DateTime.UtcNow;
            }
            MostRecentValueDate = DateTime.UtcNow;
            MostRecentValue = value;
            sumValue += value;
            if (value > maximumValue) maximumValue = value;
            if (value < minimumValue) minimumValue = value;
        }

        #endregion

        #region private stuff

        private double GetValueIfValid(double value)
        {
            return SampleSize <= 0 ? double.NaN : value;
        }

        private double GetDurationInSeconds()
        {
            TimeSpan ts = MostRecentValueDate.Subtract(FirstDate);
            return ts.TotalSeconds;
        }

        private readonly string noDesignationSpecified = "<not specified>";
        private readonly string noDataYet = "no data yet";
        private double sumValue;
        private double maximumValue;
        private double minimumValue;

        #endregion

        public override string ToString()
        {
            return SampleSize > 0
                ? string.Format("{0} : {1} ± {2}", Designation, AverageValue, Range/2.0)
                : string.Format("{0} : {1}", Designation, noDataYet );
        }
        
    }
}
