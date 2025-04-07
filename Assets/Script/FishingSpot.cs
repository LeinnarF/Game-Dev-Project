using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    [System.Serializable]
    public class FishData
    {
        public string fishName;
        public float catchChance; // 0 to 1
    }

    public FishData[] fishList;

    // Call this from Player when fishing is triggered
    public string TryCatchFish()
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var fish in fishList)
        {
            cumulative += fish.catchChance;
            if (roll <= cumulative)
            {
                return fish.fishName;
            }
        }

        return "Nothing";
    }
}
