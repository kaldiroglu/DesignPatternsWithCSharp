namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// The first shipment of stage three: paid, shipped as TR-1, and three failed delivery
/// attempts, so the parcel is back at the warehouse.
/// </summary>
internal static class SampleOrder
{
    internal static EnumOrder BackFromTheFirstShipment()
    {
        var order = new EnumOrder();
        order.Pay();
        order.Ship("TR-1");
        for (int i = 0; i < 3; i++)
        {
            order.FailDelivery();
        }
        return order;
    }
}
