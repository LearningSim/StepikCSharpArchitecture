using System;
using System.Runtime.CompilerServices;

namespace HotelAccounting;

public class AccountingModel : ModelBase
{
    private double price;
    public double Price
    {
        get => price;
        set
        {
            if (value == price) return;
            if (value < 0) throw new ArgumentException();

            price = value;
            Notify();
            UpdateTotal();
        }
    }

    private int nightsCount;
    public int NightsCount
    {
        get => nightsCount;
        set
        {
            if (value == nightsCount) return;
            if (value <= 0) throw new ArgumentException();

            nightsCount = value;
            Notify();
            UpdateTotal();
        }
    }

    private double discount;
    public double Discount
    {
        get => discount;
        set
        {
            if (value == discount) return;
            
            discount = value;
            Notify();
            UpdateTotal();
        }
    }

    private double total;
    public double Total
    {
        get => total;
        set
        {
            if (value == total) return;
            if (value < 0) throw new ArgumentException();
            
            total = value;
            Notify();
            Discount = 100 - 100 * total / Price / NightsCount;
        }
    }

    private new void Notify([CallerMemberName] string propertyName = "") => base.Notify(propertyName);
    
    private void UpdateTotal() =>
        Total = Price * NightsCount * (1 - Discount / 100);
}