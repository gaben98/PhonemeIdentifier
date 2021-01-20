using System;
using System.Collections.Generic;
using System.Linq;

namespace PhonemeIdentifier {
	class Program {
		static void Main(string[] args) {
			//https://www.phon.ucl.ac.uk/home/wells/ipa-unicode.htm
			char[] vowelNames = new char[] { 'i', 'ɪ', 'e', 'ɛ', 'æ', 'ɨ', 'ə', 'ʌ', 'a', 'ɑ', 'u', 'o', 'ʊ' };
			char[] consonantNames = new char[] { 'p', 'b', 't', 'd', 'k', 'g', 'f', 'v', 's', 'z', 'θ', 'ð', 'ʃ', 'ʒ', 'ʧ', 'ʤ', 'm', 'n', 'ŋ', 'l', 'ɹ', 'j', 'w', 'ʍ', 'h', 'ʔ' };

			bool[][] vowelTable = new bool[][] {
				new bool[] { false, true, true, false, true, false, true, false, true },//features for "i"
				new bool[] { false, true, true, false, true, false, true, false, false },//features for "ɪ"
				new bool[] { false, true, true, false, false, false, true, false, true },//features for "e"
				new bool[] { false, true, true, false, false, false, true, false, false },//features for "ɛ"
				new bool[] { false, true, true, false, false, false, true, true, false },//features for "æ"
				new bool[] { false, true, true, false, true, false, false, false, false },//features for "ɨ"
				new bool[] { false, true, true, false, false, false, false, false, false },//features for "ə"
				new bool[] { false, true, true, false, false, true, false, false, false },//features for "ʌ"
				new bool[] { false, true, true, false, false, true, false, true, true },//features for "a"
				new bool[] { false, true, true, false, false, true, false, true, true },//features for "ɑ"
				new bool[] { false, true, true, true, true, true, false, false, true },//features for "u"
				new bool[] { false, true, true, true, false, true, false, false, true },//features for "o"
				new bool[] { false, true, true, true, true, true, false, false, false }//features for "ʊ"
			};

			bool[][] consonantTable = new bool[][] {
				new bool[] { true, false, false, false, true, false, true, false, false, true, false, false },
				new bool[] { true, false, false, true, true, false, true, false, false, true, false, false },
				new bool[] { true, false, false, false, false, true, true, false, false, true, false, false },
				new bool[] { true, false, false, true, false, true, true, false, false, true, false, false },
				new bool[] { true, false, false, false, false, false, false, false, false, true, true, false },
				new bool[] { true, false, false, true, false, false, false, false, false, true, true, false },
				new bool[] { true, false, false, false, true, false, true, false, false, false, false, false },
				new bool[] { true, false, false, true, true, false, true, false, false, false, false, false },
				new bool[] { true, false, false, false, false, true, true, true, false, false, false, false },
				new bool[] { true, false, false, true, false, true, true, true, false, false, false, false },
				new bool[] { true, false, false, false, false, true, true, false, false, false, false, false },
				new bool[] { true, false, false, true, false, true, true, false, false, false, false, false },
				new bool[] { true, false, false, false, false, true, false, true, false, false, false, false },
				new bool[] { true, false, false, true, false, true, false, true, false, false, false, false },
				new bool[] { true, false, false, false, false, true, false, true, false, true, false, false },
				new bool[] { true, false, false, true, false, true, false, true, false, true, false, false },
				new bool[] { true, false, true, true, true, false, true, false, true, true, false, false },
				new bool[] { true, false, true, true, false, true, true, false, true, true, false, false },
				new bool[] { true, false, true, true, false, false, false, false, true, true, true, false },
				new bool[] { true, false, true, true, false, true, true, false, false, false, false, true },
				new bool[] { true, false, true, true, false, true, true, false, false, false, false, false },
				new bool[] { false, false, true, true, false, false, false, false, false, false, false, false },
				new bool[] { false, false, true, true, true, false, false, false, false, false, false, false },
				new bool[] { false, false, false, false, true, false, false, false, false, false, false, false },
				new bool[] { false, false, false, false, false, false, false, false, false, false, true, false },
				new bool[] { false, false, false, false, false, false, false, false, false, true, true, false }
			};

			Console.InputEncoding = System.Text.Encoding.Unicode;

			if(args.Count() == 0) {
				Console.WriteLine("PhonemeIdentifier 0.2.4");
				Console.WriteLine("For a list of commands, type \"help\"");
				Console.Write("> ");
				args = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
			}

			while(true) {
				string command = args[0];
				switch(command) {
					case "quit":
						return;
					case "identify":
						char[] phonemes = args.Skip(1).Select(s => s[0]).ToArray();
						if(phonemes.All(phoneme => vowelNames.Contains(phoneme))) FindVowelDescriptions(vowelTable, phonemes, vowelNames);
						else if(phonemes.All(phoneme => consonantNames.Contains(phoneme))) FindConsonantDescriptions(consonantTable, phonemes, consonantNames);
						else Console.WriteLine("Either the phonemes were not entered correctly or were not all from the same table.  Visit https://www.phon.ucl.ac.uk/home/wells/ipa-unicode.htm for correct unicode representations");
						break;
					case "addvowel":
						(vowelTable, vowelNames) = AddVowel(vowelTable, vowelNames, args[1][0], args[2]);
						break;
					case "addconsonant":
						(consonantTable, consonantNames) = AddConsonant(consonantTable, consonantNames, args[1][0], args[2]);
						break;
					case "features":
						ListFeatures(consonantTable, vowelTable, consonantNames, vowelNames, args[1][0]);
						break;
					case "class":
						string features = args[1];
						Class(consonantTable, vowelTable, consonantNames, vowelNames, features);
						break;
					case "parse":
						string[] tokens = args.Skip(1).ToArray();
						Parse(tokens);
						break;
					case "vowels":
						Console.WriteLine(String.Join(", ", vowelNames));
						break;
					case "consonants":
						Console.WriteLine(String.Join(", ", consonantNames));
						break;
					case "help":
						Console.WriteLine("Commands:");
						Console.WriteLine("\"identify <phoneme 1> <phoneme 2> <phoneme 3> ...\"");
						Console.WriteLine("\tfinds the minimal set of features that identifies this set of phonemes and no others, or will find that the set cannot be uniquely identified");
						Console.WriteLine("\"addvowel <phoneme> <features>\"");
						Console.WriteLine("\tadds a vowel with the specified features.  Features should be input as 9 0s or 1s, a 0 for - and a 1 for +.  IE \"addvowel i 011010101\"");
						Console.WriteLine("\"addconsonant <phoneme> <features>\"");
						Console.WriteLine("\tadds a consonant with the specified features.  Features should be input as 12 0s or 1s, a 0 for - and a 1 for +.  IE \"addconsonant p 100010100100\"");
						Console.WriteLine("\"vowels\"");
						Console.WriteLine("\tlists all vowels in the active vowel table, including added vowels.");
						Console.WriteLine("\"consonants\"");
						Console.WriteLine("\tlists all consonants in the active consonant table, including added consonants.");
						Console.WriteLine("\"help\"");
						Console.WriteLine("\tdisplays available commands");
						Console.WriteLine("\"features <phoneme>\"");
						Console.WriteLine("\tdisplays the features associated with the given phoneme");
						Console.WriteLine("\"class <features>\"");
						Console.WriteLine("\tdisplays all phonemes of the class defined by the given features.  Will be classified as a vowel or consonant based on how many features are specified");
						Console.WriteLine("\"parse <sentence>\"");
						Console.WriteLine("\tparses the given english sentence into a syntax tree.");
						Console.WriteLine("\"quit\"");
						Console.WriteLine("\tquits the program.  The table will be reset to the one provided when next ran.");
						break;
					default:
						break;
				}
				Console.Write("> ");
				args = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
			}

		}

		public static void FindVowelDescriptions(bool[][] vowelTable, char[] phonemes, char[] vowelNames) {
			int vowelFeatureCount = 9;

			var phonemeArrays = phonemes.Select(phoneme => vowelTable[Array.IndexOf(vowelNames, phoneme)]).ToArray();
			//create an array of features that maximally covers the similarities in the set of phonemes
			//so if every phoneme in the set is minus or plus for some feature, then put a minus or plus in the maximal array
			//otherwise put a wildcard
			//so by index, see if every array in phonemeArrays matches the first array's value
			FeatureValue[] maximalDescription = Enumerable.Range(0, vowelFeatureCount).ToArray().Select(index => phonemeArrays.All((features) => features[index] == phonemeArrays[0][index]) switch {
				false => FeatureValue.Wildcard,
				true => phonemeArrays[0][index] ? FeatureValue.Plus : FeatureValue.Minus
			}).ToArray();
			Console.WriteLine($"Set of phonemes given: {String.Join(", ", phonemes)}");
			Console.WriteLine($"This set of phonemes is maximally described by {DescriptionToString(maximalDescription)}");
			//sometimes, the maximal set may not actually 
			if(!IdentifiesUniquely(maximalDescription, phonemes)) {
				Console.WriteLine("This set of phonemes cannot be uniquely identified");
				return;
			}
			//starting with the maximal description, we want to generate leads by taking each definite feature and making it a wildcard,
			//and checking that it still uniquely identifies the set of phonemes

			Queue<FeatureValue[]> leads = new Queue<FeatureValue[]>();
			List<FeatureValue[]> minimalDescriptions = new List<FeatureValue[]>();
			HashSet<string> discoveredDescriptions = new HashSet<string>();//every description we've looked at thus far
			var minDescSet = new HashSet<string>();
			leads.Enqueue(maximalDescription);

			while(leads.Count > 0) {
				var lead = leads.Dequeue();
				var slead = DescriptionToString(lead);
				if(discoveredDescriptions.Contains(slead)) continue;
				discoveredDescriptions.Add(slead);
				var newLeads = Enumerable.Range(0, vowelFeatureCount).ToArray().Select(featureIndex => lead[featureIndex].id switch {
					2 => null,//if there's already a wildcard at featureIndex, there's no definite feature that can be made a wildcard
					_ => lead.Select((value, index) => featureIndex == index ? FeatureValue.Wildcard : value)//this copies the array with the one index changed
				}).Where(clead => clead != null && IdentifiesUniquely(clead.ToArray(), phonemes));//this cuts out empty leads and leads that don't uniquely identify the phoneme set

				if(newLeads.Count() == 0 && !minDescSet.Contains(slead)) {
					minDescSet.Add(slead);
					//Console.WriteLine(slead);
				} else foreach(var newLead in newLeads) leads.Enqueue(newLead.ToArray());
			}

			Console.WriteLine($"There are {minDescSet.Count} minimal descriptions of the phonemes given:");
			foreach(var minDesc in minDescSet) Console.WriteLine(minDesc);

			bool IdentifiesUniquely(FeatureValue[] description, char[] phonemeSet) {
				//true if the description matches the phonemes and no others
				int matches = 0;
				foreach(char phoneme in phonemeSet) {//check if the description matches the given phonemes
					var features = vowelTable[Array.IndexOf(vowelNames, phoneme)];
					for(int i = 0; i < vowelFeatureCount; i++) if(!description[i].Matches(features[i])) return false;//non match was found
				}
				for(int i = 0; i < vowelNames.Length; i++) {
					if(DescriptionMatchesPhonemeFeatures(description, vowelNames[i])) matches++;
				}
				return matches == phonemeSet.Length;
			}

			bool DescriptionMatchesPhonemeFeatures(FeatureValue[] description, char phoneme) {
				var features = vowelTable[Array.IndexOf(vowelNames, phoneme)];
				return description.Zip(features).Select(zipped => zipped.First.Matches(zipped.Second)).All(b => b);
			}
		}

		public static void FindConsonantDescriptions(bool[][] consonantTable, char[] phonemes, char[] consonantNames) {
			int consonantFeatureCount = 12;


			var phonemeArrays = phonemes.Select(phoneme => consonantTable[Array.IndexOf(consonantNames, phoneme)]).ToArray();
			//create an array of features that maximally covers the similarities in the set of phonemes
			//so if every phoneme in the set is minus or plus for some feature, then put a minus or plus in the maximal array
			//otherwise put a wildcard
			//so by index, see if every array in phonemeArrays matches the first array's value
			FeatureValue[] maximalDescription = Enumerable.Range(0, consonantFeatureCount).ToArray().Select(index => phonemeArrays.All((features) => features[index] == phonemeArrays[0][index]) switch {
				false => FeatureValue.Wildcard,
				true => phonemeArrays[0][index] ? FeatureValue.Plus : FeatureValue.Minus
			}).ToArray();
			Console.WriteLine($"Set of phonemes given: {String.Join(", ", phonemes)}");
			Console.WriteLine($"This set of phonemes is maximally described by {DescriptionToString(maximalDescription)}");
			//sometimes, the maximal set may not actually 
			if(!IdentifiesUniquely(maximalDescription, phonemes)) {
				Console.WriteLine("This set of phonemes cannot be uniquely identified");
				return;
			}
			//starting with the maximal description, we want to generate leads by taking each definite feature and making it a wildcard,
			//and checking that it still uniquely identifies the set of phonemes

			Queue<FeatureValue[]> leads = new Queue<FeatureValue[]>();
			List<FeatureValue[]> minimalDescriptions = new List<FeatureValue[]>();
			HashSet<string> discoveredDescriptions = new HashSet<string>();//every description we've looked at thus far
			var minDescSet = new HashSet<string>();
			leads.Enqueue(maximalDescription);

			while(leads.Count > 0) {
				var lead = leads.Dequeue();
				var slead = DescriptionToString(lead);
				if(discoveredDescriptions.Contains(slead)) continue;
				discoveredDescriptions.Add(slead);
				var newLeads = Enumerable.Range(0, consonantFeatureCount).ToArray().Select(featureIndex => lead[featureIndex].id switch {
					2 => null,//if there's already a wildcard at featureIndex, there's no definite feature that can be made a wildcard
					_ => lead.Select((value, index) => featureIndex == index ? FeatureValue.Wildcard : value)//this copies the array with the one index changed
				}).Where(clead => clead != null && IdentifiesUniquely(clead.ToArray(), phonemes));//this cuts out empty leads and leads that don't uniquely identify the phoneme set

				if(newLeads.Count() == 0 && !minDescSet.Contains(slead)) {
					minDescSet.Add(slead);
					//Console.WriteLine(slead);
				} else foreach(var newLead in newLeads) leads.Enqueue(newLead.ToArray());
			}
			//var minDescSet = new HashSet<string>(minimalDescriptions.Select(desc => DescriptionToString(desc)));
			Console.WriteLine($"There are {minDescSet.Count} minimal descriptions of the phonemes given:");
			foreach(var minDesc in minDescSet) Console.WriteLine(minDesc);

			bool IdentifiesUniquely(FeatureValue[] description, char[] phonemeSet) {
				//true if the description matches the phonemes and no others
				int matches = 0;
				foreach(char phoneme in phonemeSet) {//check if the description matches the given phonemes
					var features = consonantTable[Array.IndexOf(consonantNames, phoneme)];
					for(int i = 0; i < consonantFeatureCount; i++) if(!description[i].Matches(features[i])) return false;//non match was found
				}
				for(int i = 0; i < consonantNames.Length; i++) {
					if(DescriptionMatchesPhonemeFeatures(description, consonantNames[i])) matches++;
				}
				return matches == phonemeSet.Length;
			}

			bool DescriptionMatchesPhonemeFeatures(FeatureValue[] description, char phoneme) {
				var features = consonantTable[Array.IndexOf(consonantNames, phoneme)];
				return description.Zip(features).Select(zipped => zipped.First.Matches(zipped.Second)).All(b => b);
			}
		}

		private static string DescriptionToString(FeatureValue[] description) => String.Join(", ", description.Select(fvalue => fvalue.ToString()));

		//returns a new vowelTable and vowelNames
		private static (bool[][], char[]) AddVowel(bool[][] vowelTable, char[] vowelNames, char phoneme, string features) {
			if(vowelNames.Contains(phoneme)) {
				Console.WriteLine("Vowel already present in table.");
				return (vowelTable, vowelNames);
			} else if(features.Length != 9) {
				Console.WriteLine("Incorrect number of features specified.");
				return (vowelTable, vowelNames);
			} else {
				return (vowelTable.Append(features.Select(f => f switch {
					'1' => true,
					_ => false
				}).ToArray()).ToArray(), vowelNames.Append(phoneme).ToArray());
			}
		}

		private static (bool[][], char[]) AddConsonant(bool[][] consonantTable, char[] consonantNames, char phoneme, string features) {
			if(consonantNames.Contains(phoneme)) {
				Console.WriteLine("Vowel already present in table.");
				return (consonantTable, consonantNames);
			} else if(features.Length != 12) {
				Console.WriteLine("Incorrect number of features specified.");
				return (consonantTable, consonantNames);
			} else {
				return (consonantTable.Append(features.Select(f => f switch {
					'1' => true,
					_ => false
				}).ToArray()).ToArray(), consonantNames.Append(phoneme).ToArray());
			}
		}

		private static void ListFeatures(bool[][] consonantTable, bool[][] vowelTable, char[] consonantNames, char[] vowelNames, char phoneme) {
			if(vowelNames.Contains(phoneme)) {
				var features = vowelTable[Array.IndexOf(vowelNames, phoneme)];
				Console.WriteLine(String.Join(", ", features.Select(b => b switch {
					true => '+',
					false => '-'
				})));
			} else if(consonantNames.Contains(phoneme)) {
				var features = consonantTable[Array.IndexOf(consonantNames, phoneme)];
				Console.WriteLine(String.Join(", ", features.Select(b => b switch {
					true => '+',
					false => '-'
				})));
			} else {
				Console.WriteLine("phoneme not found in the consonant table or the vowel table.");
			}
		}

		private static void Class(bool[][] consonantTable, bool[][] vowelTable, char[] consonantNames, char[] vowelNames, string features) {
			var description = features.Select(f => f switch {
				'+' => FeatureValue.Plus,
				'-' => FeatureValue.Minus,
				_ => FeatureValue.Wildcard
			}).ToArray();
			if(features.Length == 9) {// a vowel
				var matchingPhonemes = vowelNames.Where(vowel => DescriptionMatchesPhonemeFeatures(description, vowel, vowelTable, vowelNames));
				Console.WriteLine($"there are {matchingPhonemes.Count()} phonemes in this class");
				Console.WriteLine($"{String.Join(", ", matchingPhonemes)}");
			} else if(features.Length == 12) {// a consonant
				var matchingPhonemes = consonantNames.Where(consonant => DescriptionMatchesPhonemeFeatures(description, consonant, consonantTable, consonantNames));
				Console.WriteLine($"there are {matchingPhonemes.Count()} phonemes in this class");
				Console.WriteLine($"{String.Join(", ", matchingPhonemes)}");
			} else Console.WriteLine($"{features.Length} features given, but looking for 9 for a vowel or 12 for a consonant.");

			bool DescriptionMatchesPhonemeFeatures(FeatureValue[] description, char phoneme, bool[][] table, char[] names) {
				var features = table[Array.IndexOf(names, phoneme)];
				return description.Zip(features).Select(zipped => zipped.First.Matches(zipped.Second)).All(b => b);
			}
		}

		private static void Parse(string[] tokens) {
			Dictionary<string, leaf[]> partsOfSpeech = new Dictionary<string, leaf[]>();//tracks what parts of speech each word can be
			foreach(string token in tokens) {
				if(!partsOfSpeech.ContainsKey(token)) {
					Console.WriteLine($"Please enter the possible parts of speech for [{token}], separated by spaces.  Elements are {{N, A, V, P, C, D, Aux, And}}");
					var parts = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(ch => ch.Trim() switch {
						"N" => new N(token),
						"A" => new A(token),
						"V" => new V(token),
						"P" => new P(token),
						"C" => new C(token),
						"D" => new D(token),
						"Aux" => new Aux(token),
						"And" => new leaf(token),
						_ => new leaf(token)
					}).ToArray();
					partsOfSpeech.Add(token, parts);
				}
			}

			//these are the top nodes while building the tree.  We will reuse the variable until the tree is built.
			//var topNodes = tokens.Select(token => new Node(partsOfSpeech[token])).ToArray();

			//at every level, the sentence is always partitioned by leaves
			//so at each step, I have leaf[][] where each leaf[] is bijective to a partition
			//each partition represents a way of parsing the sentence up to n times
			//a partition is represented by a set of indices that divide the set, excluding 0 and the last index
			//to build up to the next partition, I need to create nodes from each class of the partition, and that gives a new partition with less divisions
			//so each class in each partition of the tree will have a set of nodes that represent how that subset of the sentence could be parsed
			//I'm going to start with a maximum partition, every word is it's own node
			//then each step I apply rules to combine classes into one within the partition
			//so what I really want to do is start with every partition of n subsets, then find all partitions of n-1 subsets, and so on and so forth
			//ending in every partition of 1 subset, aka a built tree

			//at each step I have multiple partitions, and each subset in each partition may have multiple lexemes it can be parsed as
			//which was the motivation behind the subset memo, so that I can track what lexemes each subset can be separate from the partition possibilities

			int[] startingPartitions = Enumerable.Range(1, tokens.Length - 1).ToArray();//every word is a leaf, so every index but the first and last are included
																						//(Node[], int[])[] bijection = new(Node[], int[])[] { (topNodes, startingPartitions) };//this is an array of each bijective pair of a set of leaves and the indices that the set is partitioned by

			//we need a table of each partition of n subsets, n-1 subsets, etc

			//start with maximal partition with n cuts
			//all maximal partitions can be found by applying single-node rules to every partition in the queue
			//applying all two node rules gives every partition with n-1 cuts, and the 3 node rule gives some partitions with n-2 cuts,
			//the rest coming from applying two node rules to the n-1 cuts partitions
			Queue<Partition> possiblePartitions = new Queue<Partition>();
			//List<List<Partition>> granulatedPartitions = new List<List<Partition>>(startingPartitions.Length);//index 0 is for maximal partitions, index 1 for n-1 cut partitions, etc
																											  //so index n should have partitions with no cuts
			//for(int i = 0; i < granulatedPartitions.Count; i++) granulatedPartitions[i] = new List<Partition>();
			//from the starting parts of speech possible for each word I have to enumerate all the possible assignments of parts of speech to words
			var perms = GetAllPermutations(tokens);

			foreach(leaf[] perm in perms) {
				var partition = new Partition(startingPartitions, perm);
				possiblePartitions.Enqueue(partition);
				//granulatedPartitions[startingPartitions.Length - 1].Add(partition);
			}

			Queue<Partition> granMinusOnePartitions = new Queue<Partition>();//this will become the new queue for the next iteration
			Queue<Partition> granMinusTwoPartitions = new Queue<Partition>();//this is the queue for the second next iteration

			HashSet<string> parsings = new HashSet<string>();
			//List<Partition> solutions = new List<Partition>();

			for(int n = startingPartitions.Length + 1; n > 1; n--) {
				
				while(possiblePartitions.Count > 0) {
					//first add one node changes to the active queue to get new partitions of the same granularity
					var curPartition = possiblePartitions.Dequeue();
					for(int i = 0; i < curPartition.coloring.Length; i++) {//iterates through all lexemes in the coloring
						lexeme color = curPartition.coloring[i];
						if(color is V _v) possiblePartitions.Enqueue(new Partition(curPartition.cuts, Replace(curPartition.coloring, i, new VP1(_v))));
						if(color is N _n) possiblePartitions.Enqueue(new Partition(curPartition.cuts, Replace(curPartition.coloring, i, new NBar1(_n))));
						if(color is NBar _nb) possiblePartitions.Enqueue(new Partition(curPartition.cuts, Replace(curPartition.coloring, i, new NP1(_nb))));
					}
					//then apply two node changes to get starting partitions with one less granularity
					for(int i = 0; i < curPartition.coloring.Length - 1; i++) {
						var (firstColor, secondColor) = (curPartition.coloring[i], curPartition.coloring[i+1]);
						if(firstColor is Aux _a && secondColor is VP _vp) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new VP2(_a, _vp)));
						if(firstColor is V _v && secondColor is NP _np) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new VP3(_v, _np)));
						if(firstColor is VP _vp1 && secondColor is CP _cp) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new VP4(_vp1, _cp)));
						if(firstColor is VP _vp2 && secondColor is PP _pp) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new VP5(_vp2, _pp)));
						if(firstColor is P _p && secondColor is S _s) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new PP1(_p, _s)));
						if(firstColor is P _p1 && secondColor is NP _np1) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new PP2(_p1, _np1)));
						if(firstColor is CP _cp1 && secondColor is VP _vp3) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new S1(_cp1, _vp3)));
						if(firstColor is NP _np2 && secondColor is VP _vp4) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new S2(_np2, _vp4)));
						if(firstColor is C _c && secondColor is S _s1) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new CP1(_c, _s1)));
						if(firstColor is NBar _nb && secondColor is CP _cp2) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new NBar2(_nb, _cp2)));
						if(firstColor is NBar _nb1 && secondColor is PP _pp1) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new NBar3(_nb1, _pp1)));
						if(firstColor is A _a1 && secondColor is NBar _nb2) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new NBar4(_a1, _nb2)));
						if(firstColor is D _d && secondColor is NBar _nb3) granMinusOnePartitions.Enqueue(combineLexemes(curPartition, i, new NP2(_d, _nb3)));
					}
					//apply three node changes to get starting partitions with two less granularity
					for(int i = 0; i < curPartition.coloring.Length - 2; i++) {
						var (firstColor, secondColor, thirdColor) = (curPartition.coloring[i], curPartition.coloring[i+1], curPartition.coloring[i+2]);
						if(secondColor is leaf l && l.sign == "and") {
							if(firstColor is VP vp1 && thirdColor is VP vp2) granMinusTwoPartitions.Enqueue(combineThreeLexemes(curPartition, i, new VPCombo(vp1, vp2)));
							if(firstColor is PP pp1 && thirdColor is PP pp2) granMinusTwoPartitions.Enqueue(combineThreeLexemes(curPartition, i, new PPCombo(pp1, pp2)));
							if(firstColor is S s1 && thirdColor is S s2) granMinusTwoPartitions.Enqueue(combineThreeLexemes(curPartition, i, new SCombo(s1, s2)));
							if(firstColor is CP cp1 && thirdColor is CP cp2) granMinusTwoPartitions.Enqueue(combineThreeLexemes(curPartition, i, new CPCombo(cp1, cp2)));
							if(firstColor is NBar nbar1 && thirdColor is NBar nbar2) granMinusTwoPartitions.Enqueue(combineThreeLexemes(curPartition, i, new NBarCombo(nbar1, nbar2)));
							if(firstColor is NP np1 && thirdColor is NP np2) granMinusTwoPartitions.Enqueue(combineThreeLexemes(curPartition, i, new NPCombo(np1, np2)));
						}
					}
				}
				//we're reducing the level of granularity for the next iteration, so we want to shift the queues up to reflect this
				possiblePartitions = new Queue<Partition>();
				while(granMinusOnePartitions.Count > 0) {
					var partition = granMinusOnePartitions.Dequeue();
					if(partition.coloring.Length == 1) parsings.Add(partition.coloring[0].PrintPretty("", true));
					possiblePartitions.Enqueue(partition);
				}
				while(granMinusTwoPartitions.Count > 0) {
					var partition = granMinusTwoPartitions.Dequeue();
					if(partition.coloring.Length == 1) parsings.Add(partition.coloring[0].PrintPretty("", true));
					granMinusOnePartitions.Enqueue(granMinusTwoPartitions.Dequeue());
				}
			}

			Console.WriteLine($"{parsings.Count} parsings found:");
			foreach(var parsing in parsings) Console.WriteLine(parsing);

			T[] Replace<T>(T[] array, int index, T element) {
				var nArr = new T[array.Length];
				array.CopyTo(nArr, 0);
				nArr[index] = element;
				return nArr;
			}

			T[] combineCuts<T>(T[] cuts, int firstCutIndex, int count = 1) {//returns a new set of cuts where the cuts at the first index and the following index are combined
				var output = cuts.ToList();
				for(int i = 0; i < count; i++) output.RemoveAt(firstCutIndex + 1);
				return output.ToArray();
			}

			Partition combineLexemes(Partition input, int firstLexemeIndex, lexeme newLexeme) {
				var newLexemes = combineCuts(input.coloring, firstLexemeIndex);
				newLexemes[firstLexemeIndex] = newLexeme;
				return new Partition(combineCuts(input.cuts, firstLexemeIndex - 1), newLexemes);
			}

			Partition combineThreeLexemes(Partition input, int firstLexemeIndex, lexeme newLexeme) {
				var newLexemes = combineCuts(input.coloring, firstLexemeIndex, 2);
				newLexemes[firstLexemeIndex] = newLexeme;
				return new Partition(combineCuts(input.cuts, firstLexemeIndex - 1, 2), newLexemes);
			}

			leaf[][] GetAllPermutations(string[] tokens) {
				if(tokens.Length == 1) return partsOfSpeech[tokens[0]].Select(pos => new leaf[] { pos }).ToArray();

				var tailPerms = GetAllPermutations(tokens.Skip(1).ToArray());
				//take all the ways to parse the first token
				//and prepend to all the ways of parsing the rest of the tokens
				return partsOfSpeech[tokens[0]].SelectMany(firstPoS => tailPerms.Select(perm => perm.Prepend(firstPoS).ToArray())).ToArray();
			}
		}
	}
}
