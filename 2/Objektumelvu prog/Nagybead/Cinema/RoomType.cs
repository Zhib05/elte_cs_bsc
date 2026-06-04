using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public interface RoomType
    {
        double ApplyDiscount(double basalPrice, Status status);
    }

    public class VIP : RoomType
    {
        public double ApplyDiscount(double basePrice, Status status)
        {
            double discount = 0;
            if (status == Status.Adult)
                discount = 0.05;
            return basePrice * (1 - discount);
        }
    }

    public class Small : RoomType
    {
        public double ApplyDiscount(double basePrice, Status status)
        {
            double discount = 0;
            switch (status)
            {
                case Status.Child:
                    discount = 0.4; break;
                case Status.Student:
                    discount = 0.2; break;
                case Status.Retired:
                    discount = 0.3; break;
                case Status.Regular:
                    discount = 0.4; break;
            }
            return basePrice * (1 - discount);
        }
    }

    public class Large : RoomType
    {
        public double ApplyDiscount(double basePrice, Status status)
        {
            double discount = 0;
            switch (status)
            {
                case Status.Adult:
                    discount = 0.1; break;
                case Status.Child:
                    discount = 0.2; break;
                case Status.Student:
                    discount = 0.4; break;
                case Status.Retired:
                    discount = 0.2; break;
                case Status.Regular:
                    discount = 0.4; break;
            }
            return basePrice * (1 - discount);
        }
    }

    public class Medium : RoomType
    {
        public double ApplyDiscount(double basePrice, Status status)
        {
            double discount = 0;
            switch (status)
            {
                case Status.Adult:
                    discount = 0.05; break;
                case Status.Child:
                    discount = 0.3; break;
                case Status.Student:
                    discount = 0.3; break;
                case Status.Retired:
                    discount = 0.2; break;
                case Status.Regular:
                    discount = 0.4; break;
            }
            return basePrice * (1 - discount);
        }
    }
}
