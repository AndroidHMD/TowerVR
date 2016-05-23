using UnityEngine;
using System.Collections;

public class PlayerNameGenerator {
	public static string[] prefixes = new string[]{
		"Mr.", "Mrs.", "Dr.", "Professor", 
		"Little", "Gigantic", "Fierce", "Tiny", "Enormous", "Fat",
		"Angry", "Calm", "Crazy", "Idiotic", "Drunken",
		"Swaggy", "Derpy", "Explosive", "Destructive", "Masochistic",
		"King", "Queen", "Prince", "Princess"
	};
	
	public static string[] mainNames = new string[]{
		"Boom", "Potato", "Carrot", "Banana", "Ananas", "Apple", "Sausage",
		"Rabbit", "Bunny", "Elk", "Moose", "Bird", "Dog", "Cat", "Skeleton",
		"Master", "Noob", "Pro", "Newbie",
		"Robot", "Machine", "Fart",
		"Carl Gustaf", "Obama"
	};
	
	public static string[] suffixes = new string[]{
		" the Great", " the First", " the Second", " the Third",
		" I", " II", " III", " IV", " V", " VI", " XII", " XIV", " XVI",
		" Jr.", " Sr.",
		", PhD", ", MSc", ", BSc"
	};
	
	public static string GenerateName()
	{
		string prefix = GetRandomInArray(prefixes);
		string main = " " + GetRandomInArray(mainNames);
		
		// generate a suffix 50% of the time
		string suffix = "";
		if (Random.Range(0, 2) == 1)
		{
			suffix = GetRandomInArray(suffixes);
		}
		
		return prefix + main + suffix;
	}
	
	private static string GetRandomInArray(string[] arr)
	{
		int index = Random.Range(0, arr.Length);
		return arr[index];
	}
}
