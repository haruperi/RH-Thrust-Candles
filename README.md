# RH-Thrust Candles

This indicator for cTrader identifies "thrust" candles and colors them based on their strength. A thrust candle is a candle that closes significantly above or below the highs/lows of a specified number of preceding candles.

## How it Works

-   **Bullish Thrust:** A bullish candle is considered a "strong" thrust if its closing price is higher than the high prices of the `Look Back Candles` number of preceding candles. Otherwise, it's considered a "weak" bullish candle.
-   **Bearish Thrust:** A bearish candle is considered a "strong" thrust if its closing price is lower than the low prices of the `Look Back Candles` number of preceding candles. Otherwise, it's considered a "weak" bearish candle.
-   **Neutral Candles:** If a candle's open and close prices are the same (e.g., a Doji), its color is not changed by this indicator.

## Parameters

The indicator has the following configurable parameters:

-   **Look Back Candles:**
    -   Description: The number of previous candles to look back at to determine if the current candle is a strong thrust.
    -   Default Value: `2`
    -   Minimum Value: `1`

-   **Strong Bullish Candle Colour:**
    -   Description: The color for strong bullish thrust candles.
    -   Default Value: `#26A69A` (a shade of green)

-   **Strong Bearish Candle Colour:**
    -   Description: The color for strong bearish thrust candles.
    -   Default Value: `#EF5350` (a shade of red)

-   **Weak Bullish Candle Colour:**
    -   Description: The color for weak bullish candles (those that are bullish but not strong thrusts).
    -   Default Value: `#0B2D29` (a dark green/teal)

-   **Weak Bearish Candle Colour:**
    -   Description: The color for weak bearish candles (those that are bearish but not strong thrusts).
    -   Default Value: `#420706` (a dark red)

## Outputs

The indicator directly modifies the color of the bars on the chart. It also exposes two `IndicatorDataSeries`:

-   `ThrustCandle`: This series is declared but not explicitly assigned values within the provided `Calculate` method. It might be intended for future use or for other indicators to access thrust candle information.
-   `ThrustSwitch`: Similar to `ThrustCandle`, this series is declared but not assigned values in the current `Calculate` method.

*Note: The `Initialize` method is currently empty but can be used for any setup logic if needed in the future.*