using System.Collections.Generic;
using System.Diagnostics.Metrics;
using MinimalAPI_.NET7_.Models;

namespace MinimalAPI_.NET7_.Data
{
    public static class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>
        {
            new Coupon{Id = 1,Name = "10OFF",Percent = 10,IsActive = true},
            new Coupon{Id = 2,Name = "20OFF",Percent = 20,IsActive = false}
        };
    }
}
