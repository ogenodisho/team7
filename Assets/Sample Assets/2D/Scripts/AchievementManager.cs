using UnityEngine;
using System.Collections.Generic;
using System;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AchievementManager
{
	private string PLAY_KEY = "Play_count";
	private string DIE_KEY = "Die_count";
	private string JUMP_KEY = "Jump_count";
	private string SHOOT_KEY = "Shoot_count";

	private int play = 0;
	private int die = 0;
	private int jump = 0;
	private int shoot = 0;

	// list of all achievements
	private List<Achievement> allAchievements = new List<Achievement>();

	// list of user's unlocked achievements by achievement id
	//private List<string> userAchievements = new List<string>();

	// Singleton
	private static readonly object padlock = new object();
	private static AchievementManager instance;
	private AchievementManager() {}
	public static AchievementManager Instance
	{
		get 
		{
			lock (padlock) // thread-safety
			{
				if (instance == null)
				{
					instance = new AchievementManager();
					instance.loadAchievements();
				}
				return instance;
			}
		}
	}

	private void loadAchievements()
	{
		// test - reset saved values
		//PlayerPrefs.SetInt(PLAY_KEY, 0);
		//PlayerPrefs.SetInt(DIE_KEY, 0);
		//PlayerPrefs.SetInt(JUMP_KEY, 0);
		//PlayerPrefs.SetInt(SHOOT_KEY, 0);

		// load previous, saved values.
		play = PlayerPrefs.GetInt(PLAY_KEY, 0);
		die = PlayerPrefs.GetInt(DIE_KEY, 0);
		jump = PlayerPrefs.GetInt(JUMP_KEY, 0);
		shoot = PlayerPrefs.GetInt(SHOOT_KEY, 0);

		//Debug.Log("Achievement stats; play: " + play + "; die: " + die + "; jump: " + jump + "; shoot: " + shoot);
		
		// all achievements
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQCA", AchievementType.Play, 2, "Second Time Playing!"));
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQCQ", AchievementType.Play, 3, "Third Time Playing!"));
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQBg", AchievementType.Die, 2, "Second Time Dying!"));
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQBw", AchievementType.Die, 3, "Died 3 Times!"));
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQAw", AchievementType.Jump, 10, "10 Jumps Made!"));
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQBA", AchievementType.Shoot, 10, "10 Shots Fired!"));
		allAchievements.Add(new Achievement("CgkIltz5q7wNEAIQBQ", AchievementType.Shoot, 100, "100 Shots Fired!"));

		// for all achievements set any user unlocked achievements to unlocked
		//foreach(Achievement ach in allAchievements) {
		//	if (userAchievements.Contains(ach.getId())) {
		//		ach.setUnlocked();
		//	}
		//}
	}
	
	// returns an achivement message if just unlocked otherwise null
	public void RegisterEvent( AchievementType type )
	{
		int count = 0;
		
		switch(type)
		{
		case AchievementType.Play:
			play++;
			count = play;
			PlayerPrefs.SetInt(PLAY_KEY, play);
			break;
		case AchievementType.Die:
			die++;
			count = die;
			PlayerPrefs.SetInt(DIE_KEY, die);
			break;
		case AchievementType.Jump:
			jump++;
			count = jump;
			PlayerPrefs.SetInt(JUMP_KEY, jump);
			break;
		case AchievementType.Shoot:
			shoot++;
			count = shoot;
			PlayerPrefs.SetInt(SHOOT_KEY, shoot);
			break;
		}

		checkAchievements (type, count);

		//Debug.Log("Achievement stats; play: " + play + "; die: " + die + "; jump: " + jump + "; shoot: " + shoot);
	}

	// returns an achievement message if just unlocked otherwise null
	private void checkAchievements(AchievementType type, int count)
	{
		foreach(Achievement ach in allAchievements) {
			if (ach.getType() == type && ach.isUnlocked() == false) {
				if (count >= ach.getCountToUnlock()) {
					ach.setUnlocked();
					// unlock achievement on Google play
					Social.ReportProgress(ach.getId(), 100.0f, (bool success) => {
						// handle success or failure
					});
					//userAchievements.Add(ach.getId());
					//Debug.Log ("achievement unlocked");
				}
			}
		}
	}

	public class Achievement
	{
		private string id;
		private AchievementType type;
		private int countToUnlock;
		private bool unlocked = false;
		private string message;

		// id should be unique
		public Achievement(string id, AchievementType type, int countToUnlock, string message) {
			this.id = id;
			this.type = type;
			this.countToUnlock = countToUnlock;
			this.message = message;
		}
		
		public string getId() {
			return id;
		}
		
		public AchievementType getType() {
			return type;
		}
		
		public int getCountToUnlock() {
			return countToUnlock;
		}
		
		public bool isUnlocked() {
			return unlocked;
		}
		
		public void setUnlocked() {
			unlocked = true;
		}
		
		public string getMessage() {
			return message;
		}
	}
}

public enum AchievementType
{
	Play,
	Die,
	Jump,
	Shoot
}