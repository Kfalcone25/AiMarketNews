using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest
{
    public class ClassificationResult
    {
        public string PrimarySector { get; set; }
        public List<string> SecondarySectors { get; set; }
        public string Sentiment { get; set; }
        public double Confidence { get; set; }
        public string Reasoning { get; set; }
    }
}
