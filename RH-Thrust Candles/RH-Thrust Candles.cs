using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{

    public enum MethodType
    {
        Period,
        CloseAboveBelowMTF
    }

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale =false, AccessRights = AccessRights.None)]
    public class RHThrustCandles : Indicator
    {

        [Parameter("Method", DefaultValue = "Period")]
        public MethodType Method { get; set; }

        [Parameter("Look Back Candles", DefaultValue = 2, MinValue = 1)]
        public int LookBackCandles { get; set; }

        [Parameter("Strong Bullish Candle Colour", DefaultValue = "#26A69A")]
        public string StrongBullishColor { get; set; }

        [Parameter("Strong Bearish Candle Colour", DefaultValue = "#EF5350")]
        public string StrongBearishColor { get; set; }

        [Parameter("Weak Bullish Candle Colour", DefaultValue = "#0B2D29")]
        public string WeakBullishColor { get; set; }

        [Parameter("Weak Bearish Candle Colour", DefaultValue = "#420706")]
        public string WeakBearishColor { get; set; }

        [Parameter("Lower TimeFrame", DefaultValue = "Minute")]
        public TimeFrame LowerTimeFrame { get; set; }

        [Parameter("Higher TimeFrame", DefaultValue = "Minute5")]
        public TimeFrame HigherTimeFrame { get; set; }

 
        public IndicatorDataSeries ThrustCandle { get; set; }
        public IndicatorDataSeries ThrustSwitch { get; set; }
        private int _lastThrust;

        private Bars LowerTimeFrameBars { get; set; }
        private Bars HigherTimeFrameBars { get; set; }

        protected override void Initialize()
        {
            _lastThrust = 0;
            LowerTimeFrameBars = MarketData.GetBars(LowerTimeFrame);
            HigherTimeFrameBars = MarketData.GetBars(HigherTimeFrame);
        }

        private bool IsStrongBullish(int index)
        {
            if (Method == MethodType.Period)
            {
                if (index < LookBackCandles) return false; // Not enough data for lookback

                var currentClose = Bars.ClosePrices[index];
                for (int i = 1; i <= LookBackCandles; i++)
                {
                    if (currentClose <= Bars.HighPrices[index - i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (Method == MethodType.CloseAboveBelowMTF)
            {
                // Ensure we have enough data on both timeframes
                if (index < 1 || LowerTimeFrameBars.Count <= index || HigherTimeFrameBars.Count <= 1) return false;

                var currentLTFClose = LowerTimeFrameBars.ClosePrices[index];
                // Get the corresponding index for HTF. This needs careful mapping.
                // For simplicity, let's assume we are looking at the PREVIOUS HTF bar relative to the current chart's bar.
                // A more robust solution would involve mapping timestamps.
                var htfIndex = HigherTimeFrameBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

                // Ensure htfIndex is valid and there's a previous bar
                if (htfIndex < 1 || htfIndex >= HigherTimeFrameBars.Count) return false; 

                var previousHTFHigh = HigherTimeFrameBars.HighPrices[htfIndex - 1];
                
                return currentLTFClose > previousHTFHigh;
            }

            return false;
        }

        private bool IsStrongBearish(int index)
        {
            if (Method == MethodType.Period)
            {
                if (index < LookBackCandles) return false; // Not enough data for lookback

                var currentClose = Bars.ClosePrices[index];
                for (int i = 1; i <= LookBackCandles; i++)
                {
                    if (currentClose >= Bars.LowPrices[index - i])
                    {
                        return false;
                    }
                }
                return true;
            }

            else if (Method == MethodType.CloseAboveBelowMTF)
            {
                // Ensure we have enough data on both timeframes
                if (index < 1 || LowerTimeFrameBars.Count <= index || HigherTimeFrameBars.Count <= 1) return false;

                var currentLTFClose = LowerTimeFrameBars.ClosePrices[index];
                // Get the corresponding index for HTF.
                var htfIndex = HigherTimeFrameBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

                // Ensure htfIndex is valid and there's a previous bar
                if (htfIndex < 1 || htfIndex >= HigherTimeFrameBars.Count) return false;

                var previousHTFLow = HigherTimeFrameBars.LowPrices[htfIndex - 1];

                return currentLTFClose < previousHTFLow;
            }

            return false;
        }

        public override void Calculate(int index)
        {
            var currentOpen = Bars.OpenPrices[index];
            var currentClose = Bars.ClosePrices[index];
    

            if (currentClose > currentOpen) // Bullish candle
            {
                bool isStrong = IsStrongBullish(index);
                if (isStrong)
                {
                    Chart.SetBarColor(index, StrongBullishColor);
                    ThrustCandle[index] = 1;

                    if (_lastThrust == -1)
                    {
                        ThrustSwitch[index] = 1;
                        _lastThrust = 1;
                    }
                    

                }
                else
                {
                    Chart.SetBarColor(index, WeakBullishColor);
                    ThrustCandle[index] = 0;
                }
            }
            else if (currentClose < currentOpen) // Bearish candle
            {
                bool isStrong = IsStrongBearish(index);
                if (isStrong)
                {
                    Chart.SetBarColor(index, StrongBearishColor);
                    ThrustCandle[index] = -1;

                    if (_lastThrust == 1)
                    {
                        ThrustSwitch[index] = -1;
                        _lastThrust = -1;
                    }
                    
                }
                else
                {
                    Chart.SetBarColor(index, WeakBearishColor);
                    ThrustCandle[index] = -1;
                }
            }
            // If currentClose == currentOpen (Doji or neutral candle), color is not changed.
        }
    }
}