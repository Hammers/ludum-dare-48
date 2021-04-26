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
        PlayerBank.instance.TransferFunds();
        GetComponent<Shop>().OpenShop(() => {
            EndActivation();
            Reset();
            GameManager.Instance.EndSession();
        });
    }
}
