using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonemeIdentifier {
	public class FeatureValue {
		public readonly int id;
		private Func<bool, bool> matcher;
		private string representation;
		public FeatureValue(int Id, Func<bool, bool> Matcher, string Representation) => (id, matcher, representation) = (Id, Matcher, Representation);
		public static FeatureValue Minus = new FeatureValue(0, (i) => !i, "-");//matches false
		public static FeatureValue Plus = new FeatureValue(1, (i) => i, "+");//matches true
		public static FeatureValue Wildcard = new FeatureValue(2, (i) => true, "*");//matches any

		public override string ToString() => representation;
		public bool Matches(bool feature) => matcher(feature);
	}
}
