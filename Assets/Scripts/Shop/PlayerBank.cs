using UnityEngine;

public class PlayerBank : MonoBehaviour
{
    public static PlayerBank instance;

    public int coins;

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
        inventory.ClearCoins();
    }
}
