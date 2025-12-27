using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace MagniFile {
	/// <summary>
	/// Arguments class
	/// 
	/// Code from CodeProject "C#/.NET Command Line Arguments Parser
	/// By GriffonRL
	/// 
	/// http://www.codeproject.com/KB/recipes/command_line.aspx
	/// 
	/// </summary>
	public class Arguments {
		private StringDictionary Parameters;

		public Arguments(string[] Args) {
			Parameters = new StringDictionary();
			Regex Spliter = new Regex(@"^-{1,2}|^/|=",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

			Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
				RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string Parameter = null;
			string[] Parts;

			// Valid parameters forms:
			//  {-,/,--}param{ ,=}((",')value(",'))

			// Examples: 
			//  -param1 value1 --param2 /param3:"Test-:-work" 
			//   /param4=happy -param5 '--=nice=--'

			foreach (string Txt in Args) {
				// Look for new parameters (-,/ or --) and a
				// possible enclosed value (=,:)

				Parts = Spliter.Split(Txt, 3);

				switch (Parts.Length) {
					case 1:
						// Found a value (for the last parameter 
						// found (space separator))
						if (Parameter != null) {
							if (!Parameters.ContainsKey(Parameter)) {
								Parts[0] = Remover.Replace(Parts[0], "$1");
								Parameters.Add(Parameter, Parts[0]);
							}
							Parameter = null;
						}
						// else Error: no parameter waiting for a value (skipped)

						break;

					case 2:
						// Found just a parameter
						// The last parameter is still waiting. 
						// With no value, set it to true.

						if (Parameter != null) {
							if (!Parameters.ContainsKey(Parameter))
								Parameters.Add(Parameter, "true");
						}
						Parameter = Parts[1];
						break;

					case 3:
						// Parameter with enclosed value
						// The last parameter is still waiting. 
						// With no value, set it to true.

						if (Parameter != null) {
							if (!Parameters.ContainsKey(Parameter))
								Parameters.Add(Parameter, "true");
						}

						Parameter = Parts[1];

						// Remove possible enclosing characters (",')

						if (!Parameters.ContainsKey(Parameter)) {
							Parts[2] = Remover.Replace(Parts[2], "$1");
							Parameters.Add(Parameter, Parts[2]);
						}

						Parameter = null;
						break;
				}
			}
			// In case a parameter is still waiting

			if (Parameter != null) {
				if (!Parameters.ContainsKey(Parameter))
					Parameters.Add(Parameter, "true");
			}
		}

		// Retrieve a parameter value if it exists 
		// (overriding C# indexer property)

		public string this[string Param] {
			get {
				return (Parameters[Param]);
			}
		}

		public bool TryAndGet(string param, ref string value) {
			if (Parameters.ContainsKey(param)) {
				value = Parameters[param];
				return true;
			}

			return false;
		}

		public bool TryAndGet(string param, ref int value) {
			if (Parameters.ContainsKey(param)) {
				string strValue = Parameters[param];
				try {
					value = strValue.Length > 0 ? int.Parse(strValue) : 0;
					return true;
				} catch { }
			}

			return false;
		}

	}
}
