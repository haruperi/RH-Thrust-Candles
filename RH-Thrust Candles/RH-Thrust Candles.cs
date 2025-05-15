using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale =false, AccessRights = AccessRights.None)]
    public class RHThrustCandles : Indicator
    {
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

 
        public IndicatorDataSeries ThrustCandle { get; set; }
        public IndicatorDataSeries ThrustSwitch { get; set; }

        protected override void Initialize()
        {
            // Initialization logic can be added here if needed in the future.
        }

        private bool IsStrongBullish(int index)
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

        private bool IsStrongBearish(int index)
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

        public override void Calculate(int index)
        {
            var currentOpen = Bars.OpenPrices[index];
            var currentClose = Bars.ClosePrices[index];
    

            if (currentClose > currentOpen) // Bullish candle
            {
                bool isStrong = IsStrongBullish(index);
                Chart.SetBarColor(index, isStrong ? StrongBullishColor : WeakBullishColor);
            }
            else if (currentClose < currentOpen) // Bearish candle
            {
                bool isStrong = IsStrongBearish(index);
                Chart.SetBarColor(index, isStrong ? StrongBearishColor : WeakBearishColor);
            }
            // If currentClose == currentOpen (Doji or neutral candle), color is not changed.
        }
    }
}