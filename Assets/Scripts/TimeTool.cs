using System;

public static class TimeTool
{
    private const string Day_Short = "d";
    private const string Hour_Short = "h";
    private const string Minute_Short = "m";
    private const string Second_Short = "s";

    public static string Format(this TimeSpan time)
    {
        var count = 0;
        var res = "";

        if (time.Days >= 1)
        {
            count++;
            res += $"{time.Days}{Day_Short}";
        }

        if (time.Hours >= 1)
        {
            if (count > 0) res += " ";
            count++;
            res += $"{time.Hours}{Hour_Short}";
        }

        if (time.Minutes >= 1)
        {
            if (count > 0) res += " ";
            count++;
            res += $"{time.Minutes}{Minute_Short}";
        }

        if (time.Seconds >= 1)
        {
            if (count > 0) res += " ";
            count++;
            res += $"{time.Seconds}{Second_Short}";
        }

        return res;
    }
}
