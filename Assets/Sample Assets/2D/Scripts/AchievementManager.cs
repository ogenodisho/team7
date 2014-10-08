using UnityEngine;
using System.Collections.Generic;
using System;

public class AchievementManager
{
	private int play = 0;
	private int die = 0;
	private int jump = 0;
	private int shoot = 0;

	// list of all achievements
	private List<Achievement> allAchievements = new List<Achievement>();

	// list of user's unlocked achievements by achievement id
	private List<int> userAchievements = new List<int>();

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
		// load here previous, saved values.
		// e.g.
		// play = 0
		// die = 1
		// jump = 12
		// shoot = 1231
		
		// all achievements
		allAchievements.Add(new Achievement(1, AchievementType.Play, 1, "First Time Playing!"));
		allAchievements.Add(new Achievement(2, AchievementType.Die, 1, "First Time Dying!"));
		allAchievements.Add(new Achievement(3, AchievementType.Jump, 1, "Fly High... First Leap of Faith!" ));
		allAchievements.Add(new Achievement(5, AchievementType.Shoot, 5, "5 Shots Fired! Keep it up!"));
		allAchievements.Add(new Achievement(6, AchievementType.Shoot, 10, "10 Shots Fired!"));
		allAchievements.Add(new Achievement(7, AchievementType.Play, 3, "Have you had enough of this game yet?"));

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
			break;
		case AchievementType.Die:
			die++;
			count = die;
			break;
		case AchievementType.Jump:
			jump++;
			count = jump;
			break;
		case AchievementType.Shoot:
			shoot++;
			count = shoot;
			break;
		}

		string message = checkAchievements (type, count);
		if (message != null) {
			message = "Achievement Unlocked!\n\n" + message;
			achievementQueue.Add(message);
		}

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