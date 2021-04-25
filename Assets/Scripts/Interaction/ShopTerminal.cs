public class ShopTerminal : Terminal
{
    protected override void UseTerminal()
    {
        PlayerBank.instance.TransferFunds();
        GetComponent<Shop>().OpenShop(() => Reset());
        EndActivation();
    }
}
