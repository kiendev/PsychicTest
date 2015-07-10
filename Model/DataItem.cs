using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychicTest.Model
{
    public class DataItem
    {
        public DataItem(string title)
        {
            Title = title;
        }

        public string Title
        {
            get;
            private set;
        }
    }

    public class CardItem
    {
        public int CardId { get; set; }
        public string CardThumb { get; set; }
        public string CardThumbLager { get; set; }
        public string CardKey { get; set; }
    }

    public class ColorItem
    {
        public int ColorId { get; set; }
        public string ColorThumb { get; set; }
        public string ColorThumbLager { get; set; }
        public string ColorKey { get; set; }
    }

    public class ScoreItem
    {
        public int ScoreId { get; set; }
        public string PlayerName { get; set; }
        public string Score { get; set; }
        public double RatioValue { get; set; }
        public DateTime Time { get; set; }
    }
}
