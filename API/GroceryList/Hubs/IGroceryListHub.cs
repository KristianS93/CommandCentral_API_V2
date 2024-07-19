namespace API.GroceryList.Hubs;

public interface IGroceryListHub
{
    Task RecieveMSG(string message);
    Task ConnectGroceryList(string eventItem, string msg);

    Task AddItem(string eventItem, string msg);
}