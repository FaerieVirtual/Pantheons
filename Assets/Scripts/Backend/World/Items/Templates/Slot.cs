public class Slot<T> where T : IItem
{
    private T _item;
    public bool isEmpty;
    public bool isLocked;

    public bool AddItem(T item) 
    {
        if (isLocked)  return false; 
        if (_item == null) 
        { 
            _item = item; 
            isEmpty = false;
            return true; 
        }
        else return false;
    }
    public bool RemoveItem() 
    {
        if (isLocked) return false;
        if (_item != null)
        {
            _item = default;
            isEmpty = true;
            return true;
        }
        else return false;
    }
    public T GetItem() => _item;

     
}

