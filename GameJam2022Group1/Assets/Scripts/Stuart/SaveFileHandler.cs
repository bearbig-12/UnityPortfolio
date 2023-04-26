
using System.IO;
using UnityEngine;

namespace StuartH
{
	/// <summary>
	///SaveFileHandler - Handles the saving and loading of the highscores
	/// </summary>
	public static class SaveFileHandler
	{
		const string dir = "/SaveData/";
		private static string GetPathWithFile() => Application.persistentDataPath + "/gamedata.json";
		private static string GetPath() => Application.persistentDataPath  ;

		public static void Save(ScoreHolder scoreHolder)
		{
			var json = JsonUtility.ToJson(scoreHolder.scores);
			File.WriteAllText(GetPathWithFile(), json);
		}

		public static void Load(ScoreHolder scoreHolder)
		{
			if (!Directory.Exists(GetPath())) return ;
			if (!File.Exists(GetPathWithFile())) return ;
			var x = File.ReadAllText(GetPathWithFile());
			var sh = JsonUtility.FromJson<ScoreHolder.ScoreSaveDataList>(x);
			scoreHolder.LoadScores(sh);
		}
	}

	
}