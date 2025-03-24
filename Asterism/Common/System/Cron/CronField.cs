using System.Collections.Generic;

namespace Asterism.System.Cron
{
    public class CronField
    {
        public readonly HashSet<int> _validValues;
        public string Field { get; }
        public int Min { get; }
        public int Max { get; }
        public CronField(string field, int min, int max)
        {
            Field = field;
            Min = min;
            Max = max;
            _validValues = ParseField(field, min, max);
        }

        public bool IsMatch(int value)
        {
            return _validValues.Contains(value);
        }

        private HashSet<int> ParseField(string field, int min, int max)
        {
            var values = new HashSet<int>();

            foreach (var part in field.Split(','))
            {
                if (part == "*")
                {
                    for (int i = min; i <= max; i++) values.Add(i);
                }
                else if (part.Contains("/"))
                {
                    var split = part.Split('/');
                    var rangePart = split[0];
                    var step = int.Parse(split[1]);

                    int rangeStart = min;
                    int rangeEnd = max;

                    if (rangePart != "*")
                    {
                        var rangeSplit = rangePart.Split('-');
                        rangeStart = int.Parse(rangeSplit[0]);
                        rangeEnd = int.Parse(rangeSplit[1]);
                    }

                    for (int i = rangeStart; i <= rangeEnd; i += step)
                        values.Add(i);
                }
                else if (part.Contains("-"))
                {
                    var split = part.Split('-');
                    int start = int.Parse(split[0]);
                    int end = int.Parse(split[1]);
                    for (int i = start; i <= end; i++)
                        values.Add(i);
                }
                else
                {
                    values.Add(int.Parse(part));
                }
            }

            return values;
        }
    }
}
