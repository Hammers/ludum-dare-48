public class ShopTerminal : Terminal
{
    public override void UseTerminal()
    {
        GetComponent<Shop>().OpenShop(() => Reactivate());
    }
}
