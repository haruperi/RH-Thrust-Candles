# RH-Thrust Candles

This indicator for cTrader identifies "thrust" candles and colors them based on their strength. A thrust candle is a candle that closes significantly above or below the highs/lows of a specified number of preceding candles.

## How it Works

-   **Bullish Thrust:** A bullish candle is considered a "strong" thrust if its closing price is higher than the high prices of the `Look Back Candles` number of preceding candles. Otherwise, it's considered a "weak" bullish candle.
-   **Bearish Thrust:** A bearish candle is considered a "strong" thrust if its closing price is lower than the low prices of the `Look Back Candles` number of preceding candles. Otherwise, it's considered a "weak" bearish candle.
-   **Neutral Candles:** If a candle's open and close prices are the same (e.g., a Doji), its color is not changed by this indicator.

A private variable `_lastThrust` keeps track of the direction of the last strong thrust (1 for bullish, -1 for bearish, 0 initially).

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

The indicator directly modifies the color of the bars on the chart. It also exposes two `IndicatorDataSeries` that can be used by other indicators or cBots:

-   **`ThrustCandle`**: This series provides information about the type of candle identified:
    -   `1`: Strong Bullish Thrust Candle.
    -   `0`: Weak Bullish Candle.
    -   `-1`: Strong Bearish Thrust Candle or Weak Bearish Candle. (Note: Weak bearish candles are also assigned -1. This might be an area for future refinement if a separate value for weak bearish is desired, e.g., 0 or a different negative number.)

-   **`ThrustSwitch`**: This series signals a change in the direction of strong thrusts:
    -   `1`: A strong bullish thrust occurred, and the *previous* strong thrust was bearish.
    -   `-1`: A strong bearish thrust occurred, and the *previous* strong thrust was bullish.
    -   `0` (or no value): No switch in strong thrust direction occurred on this candle.

*Note: The `Initialize` method now initializes an internal variable `_lastThrust` to 0.*