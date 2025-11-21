namespace Core.Interfaces
{
   
    public interface ICurrencyManager
    {
   
        int CurrentMoney { get; }
        
        void AddMoney(int amount);
        
        bool SpendMoney(int amount);
        
        bool HasMoney(int amount);
        
        void SetMoney(int amount);
    }
}