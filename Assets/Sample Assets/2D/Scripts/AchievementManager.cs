using UnityEngine;
using System.Collections.Generic;
using System;

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
	private List<int> userAchievements = new List<int>();

	// list of recently unlocked achievements that have yet to be displayed
	private List<string> achievementQueue = new List<string> ();
	public List<string> getQueue() {
		return achievementQueue;
	}
	public void queueDequeue() {
		Debug.Log ("dequeued");
		achievementQueue.RemoveAt(0);
	}

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
		PlayerPrefs.SetInt(PLAY_KEY, 0);
		PlayerPrefs.SetInt(DIE_KEY, 0);
		PlayerPrefs.SetInt(JUMP_KEY, 0);
		PlayerPrefs.SetInt(SHOOT_KEY, 0);

		// load previous, saved values.
		play = PlayerPrefs.GetInt(PLAY_KEY, 0);
		die = PlayerPrefs.GetInt(DIE_KEY, 0);
		jump = PlayerPrefs.GetInt(JUMP_KEY, 0);
		shoot = PlayerPrefs.GetInt(SHOOT_KEY, 0);

		Debug.Log("Achievement stats; play: " + play + "; die: " + die + "; jump: " + jump + "; shoot: " + shoot);
		
		// all achievements
		allAchievements.Add(new Achievement(1, AchievementType.Play, 1, "First Time Playing!")); // first play
		allAchievements.Add(new Achievement(2, AchievementType.Die, 1, "First Time Dying!")); // first death
		allAchievements.Add(new Achievement(3, AchievementType.Jump, 1, "Fly High... First Leap of Faith!" )); // first jump
		allAchievements.Add(new Achievement(5, AchievementType.Shoot, 5, "5 Shots Fired! Keep it up!")); // 5 shots fired
		allAchievements.Add(new Achievement(6, AchievementType.Shoot, 10, "10 Shots Fired!")); // 10 shots fired
		allAchievements.Add(new Achievement(7, AchievementType.Play, 3, "Have you had enough of this game yet?")); // 3rd time playing

		// for all achievements set any user unlocked achievements to unlocked
		foreach(Achievement ach in allAchievements) {
			if (userAchievements.Contains(ach.getId())) {
				ach.setUnlocked();
			}
		}
	}

	// returns an achivement message if just unlocked otherwise null
	public string RegisterEvent( AchievementType type )
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

		string message = checkAchievements (type, count);
		if (message != null) {
			message = "Achievement Unlocked!\n\n" + message;
			achievementQueue.Add(message);
		}

		Debug.Log("Achievement stats; play: " + play + "; die: " + die + "; jump: " + jump + "; shoot: " + shoot);
		return message;
	}

	// returns an achievement message if just unlocked otherwise null
	public string checkAchievements(AchievementType type, int count)
	{
		foreach(Achievement ach in allAchievements) {
			if (ach.getType() == type && ach.isUnlocked() == false) {
				if (count >= ach.getCountToUnlock()) {
					ach.setUnlocked();
					userAchievements.Add(ach.getId());
					Debug.Log ("achievement unlocked");
					return ach.getMessage();
				}
			}
		}
		return null;
	}

	public class Achievement
	{
		private int id;
		private AchievementType type;
		private int countToUnlock;
		private bool unlocked = false;
		private string message;

		// id should be unique
		public Achievement(int id, AchievementType type, int countToUnlock, string message) {
			this.id = id;
			this.type = type;
			this.countToUnlock = countToUnlock;
			this.message = message;
		}
		
		public int getId() {
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