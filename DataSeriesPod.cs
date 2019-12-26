using System;

namespace At.Matus.DataSeriesPod
{
    public class DataSeriesPod
    {

        #region Ctor

        public DataSeriesPod(string name)
        {
            Name = name.Trim();
            if (string.IsNullOrEmpty(Name))
                Name = noNameSpecified;
            Restart();
        }

        public DataSeriesPod() : this("") { }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public long SampleSize { get; private set; }

        public double AverageValue { get; private set; }
        public double FirstValue { get; private set; }
        public double MostRecentValue { get; private set; }
        public double MaximumValue { get; private set; }
        public double MinimumValue { get; private set; }
        public double Range => MaximumValue - MinimumValue;
        public double CentralValue => (MaximumValue + MinimumValue) / 2.0;

        public DateTime FirstDate { get; private set; }
        public DateTime MostRecentValueDate { get; private set; }
        public double Duration => MostRecentValueDate.Subtract(FirstDate).TotalSeconds;

        #endregion

        #region Methods

        public void Restart()
        {
            SampleSize = 0;
            AverageValue = double.NaN;
            MaximumValue = double.NaN;
            MinimumValue = double.NaN;
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
                AverageValue = value;
                MaximumValue = value;
                MinimumValue = value;
            }
            MostRecentValueDate = DateTime.UtcNow;
            MostRecentValue = value;
            // https://diego.assencio.com/?index=c34d06f4f4de2375658ed41f70177d59
            AverageValue += (value - AverageValue) / SampleSize;
            if (value > MaximumValue) MaximumValue = value;
            if (value < MinimumValue) MinimumValue = value;
        }

        public override string ToString() => SampleSize > 0
            ? $"{Name} : {AverageValue} ± {Range / 2.0}"
            : $"{Name} : {noDataYet}";

        #endregion

        #region private stuff

        private const string noNameSpecified = "<name not specified>";
        private const string noDataYet = "no data yet";

        #endregion

    }
}
