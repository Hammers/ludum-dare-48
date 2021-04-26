using System.Drawing;

public class ShopTerminal : Terminal
{
    public override void Start()
    {
        base.Start();
        FindObjectOfType<ShopPointer>().Setup(transform);
    }
    
    protected override void UseTerminal()
    {
        bool hadCoins = FindObjectOfType<CharacterInventory>().coins > 0;
        PlayerBank.instance.TransferFunds();
        GetComponent<Shop>().OpenShop(hadCoins,() => {
            EndActivation();
            Reset();
            GameManager.Instance.EndSession();
        });
    }
}
