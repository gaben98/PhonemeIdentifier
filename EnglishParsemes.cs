using System;
using System.Collections.Generic;
using System.Text;

namespace PhonemeIdentifier {
	public record lexeme {
		public virtual lexeme[] GetChildren() => new lexeme[0];
		public virtual string GetName() => "lexeme";
		
		public string PrintPretty(string indent, bool last) {
			var Children = GetChildren();
			string output = indent;
			//Console.Write(indent);
			if(last) {
				output += "\\-";
				//Console.Write("\\-");
				indent += "  ";
			} else {
				output += "|-";
				//Console.Write("|-");
				indent += "| ";
			}
			output += GetName() + "\n";
			//Console.WriteLine(GetName());

			for(int i = 0; i < Children.Length; i++)
				output += Children[i].PrintPretty(indent, i == Children.Length - 1);
			//Children[i].PrintPretty(indent, i == Children.Length - 1);
			return output;
		}
	}
	public record leaf(string sign) : lexeme {
		public override string GetName() => $"leaf {{{sign}}}";
	}
	public record N(string sign) : leaf(sign) {
		public override string GetName() => $"N {{{sign}}}";
	}
	public record A(string sign) : leaf(sign) {
		public override string GetName() => $"A {{{sign}}}";
	}
	public record V(string sign) : leaf(sign) {
		public override string GetName() => $"V {{{sign}}}";
	}
	public record P(string sign) : leaf(sign) {
		public override string GetName() => $"P {{{sign}}}";
	}
	public record C(string sign) : leaf(sign) {
		public override string GetName() => $"C {{{sign}}}";
	}
	public record D(string sign) : leaf(sign) {
		public override string GetName() => $"D {{{sign}}}";
	}
	public record Aux(string sign) : leaf(sign) {
		public override string GetName() => $"Aux {{{sign}}}";
	}

	public record VP : lexeme {
		public override string GetName() => "VP";
	}
	public record VP1(V s1) : VP {
		public override lexeme[] GetChildren() => new lexeme[] { s1 };
	}
	public record VP2(Aux s1, VP s2) : VP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record VP3(V s1, NP s2) : VP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record VP4(VP s1, CP s2) : VP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record VP5(VP s1, PP s2) : VP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record VPCombo(VP s1, VP s2) : VP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, new leaf("and"), s2 };
	}

	public record PP : lexeme {
		public override string GetName() => "PP";
	}
	public record PP1(P s1, S s2) : PP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record PP2(P s1, NP s2) : PP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record PPCombo(PP s1, PP s2) : PP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, new leaf("and"), s2 };
	}

	public record S : lexeme {
		public override string GetName() => "S";
	}
	public record S1(CP s1, VP s2) : S {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record S2(NP s1, VP s2) : S {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record SCombo(S s1, S s2) : S {
		public override lexeme[] GetChildren() => new lexeme[] { s1, new leaf("and"), s2 };
	}

	public record CP : lexeme {
		public override string GetName() => "CP";
	}
	public record CP1(C s1, S s2) : CP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record CPCombo(CP s1, CP s2) : CP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, new leaf("and"), s2 };
	}

	public record NBar : lexeme {
		public override string GetName() => "NBar";
	}
	public record NBar1(N s1) : NBar {
		public override lexeme[] GetChildren() => new lexeme[] { s1 };
	}
	public record NBar2(NBar s1, CP s2) : NBar {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record NBar3(NBar s1, PP s2) : NBar {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record NBar4(A s1, NBar s2) : NBar {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record NBarCombo(NBar s1, NBar s2) : NBar {
		public override lexeme[] GetChildren() => new lexeme[] { s1, new leaf("and"), s2 };
	}

	public record NP : lexeme {
		public override string GetName() => "NP";
	}
	public record NP1(NBar s1) : NP {
		public override lexeme[] GetChildren() => new lexeme[] { s1 };
	}
	public record NP2(D s1, NBar s2) : NP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, s2 };
	}
	public record NPCombo(NP s1, NP s2) : NP {
		public override lexeme[] GetChildren() => new lexeme[] { s1, new leaf("and"), s2 };
	}

	public record Partition(int[] cuts, lexeme[] coloring);
}
