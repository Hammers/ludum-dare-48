using UnityEngine;

public class PlayerBank : MonoBehaviour
{
    public static PlayerBank instance;

    public int endGameThreshold = 2000;
    
    public int coins;

    public bool passedEndGameThreshold = false;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerBank.instance != null){
            Destroy(gameObject);
            return;
        }

        PlayerBank.instance = this;
    }

    public void TransferFunds()
    {
        var inventory = FindObjectOfType<CharacterInventory>();
        coins += inventory.GetCoins();
        if (coins > endGameThreshold)
        {
            passedEndGameThreshold = true;
        }
        inventory.ClearCoins();
    }
}
