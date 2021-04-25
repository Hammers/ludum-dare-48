public class ShopTerminal : Terminal
{
    protected override void UseTerminal()
    {
        GetComponent<Shop>().OpenShop(() => Reset());
    }
}
