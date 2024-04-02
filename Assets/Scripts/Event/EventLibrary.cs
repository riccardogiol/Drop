public class StageStartEvent : Unity.Services.Analytics.Event
{
	public StageStartEvent() : base("StageStart")
	{
	}

	public int StageID { set { SetParameter("StageID", value); } }
	public int LevelID { set { SetParameter("LevelID", value); } }
	public int StageInstanceCode { set { SetParameter("StageInstanceCode", value); } }
	public float CameraRatio { set { SetParameter("CameraRatio", value); } }
}

public class GameOverEvent : Unity.Services.Analytics.Event
{
	public GameOverEvent() : base("GameOver")
	{
	}

	public int StageID { set { SetParameter("StageID", value); } }
	public int LevelID { set { SetParameter("LevelID", value); } }
	public int StageInstanceCode { set { SetParameter("StageInstanceCode", value); } }
	public float TimeElapsed { set { SetParameter("TimeElapsed", value); } }
	public float PlayerPositionX { set { SetParameter("PlayerPositionX", value); } }
	public float PlayerPositionY { set { SetParameter("PlayerPositionY", value); } }
	public int ScoutCloudUsage { set { SetParameter("ScoutCloudUsage", value); } }
	public int WaterBulletUsage { set { SetParameter("WaterBulletUsage", value); } }
	public int WaveUsage { set { SetParameter("WaveUsage", value); } }
	public int HealthLeft { set { SetParameter("HealthLeft", value); } }
	public string GameOverCode { set { SetParameter("GameOverCode", value); } }
}

public class StageCompleteEvent : Unity.Services.Analytics.Event
{
	public StageCompleteEvent() : base("StageComplete")
	{
	}

	public int StageID { set { SetParameter("StageID", value); } }
	public int LevelID { set { SetParameter("LevelID", value); } }
	public int StageInstanceCode { set { SetParameter("StageInstanceCode", value); } }
	public float TimeElapsed { set { SetParameter("TimeElapsed", value); } }
	public float PlayerPositionX { set { SetParameter("PlayerPositionX", value); } }
	public float PlayerPositionY { set { SetParameter("PlayerPositionY", value); } }
	public int ScoutCloudUsage { set { SetParameter("ScoutCloudUsage", value); } }
	public int WaterBulletUsage { set { SetParameter("WaterBulletUsage", value); } }
	public int WaveUsage { set { SetParameter("WaveUsage", value); } }
	public int HealthLeft { set { SetParameter("HealthLeft", value); } }
}

public class StageRestartEvent : Unity.Services.Analytics.Event
{
	public StageRestartEvent() : base("StageRestart")
	{
	}

	public int StageID { set { SetParameter("StageID", value); } }
	public int LevelID { set { SetParameter("LevelID", value); } }
	public int StageInstanceCode { set { SetParameter("StageInstanceCode", value); } }
	public float TimeElapsed { set { SetParameter("TimeElapsed", value); } }
	public float PlayerPositionX { set { SetParameter("PlayerPositionX", value); } }
	public float PlayerPositionY { set { SetParameter("PlayerPositionY", value); } }
	public int ScoutCloudUsage { set { SetParameter("ScoutCloudUsage", value); } }
	public int WaterBulletUsage { set { SetParameter("WaterBulletUsage", value); } }
	public int WaveUsage { set { SetParameter("WaveUsage", value); } }
	public int HealthLeft { set { SetParameter("HealthLeft", value); } }
}

