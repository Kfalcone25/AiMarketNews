using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.Models
{
	public class MarketNewsAnalysisResult
	{
		public string PrimarySector { get; set; } = string.Empty;
		public List<string> SecondarySectors { get; set; } = new();
		public List<string> AffectedCompanies { get; set; } = new();
		public string Sentiment { get; set; } = string.Empty;
		public double Confidence { get; set; }
		public string MarketImpactSummary { get; set; } = string.Empty;
		public List<string> KeyDrivers { get; set; } = new();
		public List<string> RisksOrCaveats { get; set; } = new();
		public string Reasoning { get; set; } = string.Empty;
	}
}
