using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class OrderDetailDAO
    {
        BirthdayPartyBookingContext myDB = new BirthdayPartyBookingContext();
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid id)
        {
            List<OrderDetail> orderDetails;
            try
            {
                orderDetails = await myDB.OrderDetails.AsNoTracking().Where(o => o.OrderId == id)
                                                      .Include(o => o.Service)
                                                      .Include(o => o.Service.ServiceType).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetails;
        }
        public void AddNew(OrderDetail orderDetail)
        {
            try
            {
                myDB.OrderDetails.Add(orderDetail);
                myDB.SaveChanges();
                myDB.Entry(orderDetail).State = EntityState.Detached;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
