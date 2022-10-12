using System.Reflection;

namespace Test;

public class ResourceHelper
{
	public static string? ReadResource(string name)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourcePath = name;

		resourcePath = assembly.GetManifestResourceNames()
		                       .SingleOrDefault(str => str.EndsWith(name));

		if (resourcePath == null)
		{
			return null;
		}
		
		using (var stream = assembly.GetManifestResourceStream(resourcePath))
		using (var reader = new StreamReader(stream))
		{
			return reader.ReadToEnd();
		}
	}

	public static IEnumerable<(string Name, string Content)> ReadSeqResources(string namePattern, int min, int max)
	{
		for (var i = min; i <= max; i++)
		{
			var name = string.Format(namePattern, i);
			var src = ReadResource(name);
			if (src == null)
			{
				yield break;
			}
			
			yield return (name, src);
		}
	}
}