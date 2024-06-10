using BusinessObject;
using BusinessObject.ReportData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class OrderDAO
    {
        BirthdayPartyBookingContext myDB = new BirthdayPartyBookingContext();
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<List<Order>> GetAllActiveorders()
        {
            List<Order> orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Where(s => s.DeleteFlag == 0).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public async Task<List<Order>> GetOrderForReport()
        {
            List<Order> orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Include(s => s.Host).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public async Task<List<Order>> GetOrderByDate(DateTime startDate, DateTime endDate)
        {
            List<Order> orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Where(s=> s.Date >= startDate && s.Date <= endDate).Include(s => s.Host).OrderByDescending(s => s.Status).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }
        public async Task<List<Order>> GetOrdersByStatus(int status)
        {
            return await myDB.Orders.Include(s => s.Host)
                                    .Where(o => o.Status == status)
                                    .ToListAsync();
        }
        public IQueryable<Order> GetOrderByHostID(string id)
        {
            try
            {
                var orders = myDB.Orders.AsNoTracking().Where(o => o.HostId.ToString() == id)
                                               .Include(o => o.Guest)
                                               .Include(o => o.Place);
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<HostOrderSummary>> GetHostOrderSummaries()
        {
            var hostOrderSummaries = await myDB.Orders
                .Where(o => o.Status != 5 && o.Status !=2) 
                .GroupBy(o => o.Host.Name)
                .Select(g => new HostOrderSummary
                {
                    HostName = g.Key,
                    NumberOrder = g.Count(),
                    TotalOrderMoney = g.Sum(o => o.TotalPrice)
                })
                .ToListAsync();

            return hostOrderSummaries;
        }

        public async Task<List<HostOrderSummary>> GetHostOrderSummariesByDate(DateTime startDate, DateTime endDate)
        {
            var hostOrderSummaries = await myDB.Orders
                .Where(o => o.Status != 5 && o.Status !=2 & o.Date >= startDate & o.Date <= endDate)
                .GroupBy(o => o.Host.Name)
                .Select(g => new HostOrderSummary
                {
                    HostName = g.Key,
                    NumberOrder = g.Count(),
                    TotalOrderMoney = g.Sum(o => o.TotalPrice)
                })
                .ToListAsync();

            return hostOrderSummaries;
        }

        public IQueryable<Order> GetOrderByCustomerID(string id)
        {
            try
            {
                var orders = myDB.Orders.AsNoTracking().Where(o => o.GuestId.ToString() == id).OrderByDescending(s => s.Date);
                                               
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Order> GetOrderByOrderID(Guid id)
        {
            Order orders;
            try
            {
                orders = await myDB.Orders.AsNoTracking().Include(o => o.Guest)
                                          .Include(o => o.Place).FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public bool CheckOrderExist(Order order)
        {
            bool check = false;
            try
            {
                check = myDB.Orders.AsNoTracking().Any(o => o.Date == order.Date && o.PlaceId == order.PlaceId && o.Status != 6 && o.Status != 2);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return check;
        }

        public void AddNew(Order order)
        {
            try
            {
                myDB.Orders.Add(order);
                myDB.SaveChanges();
                myDB.Entry(order).State = EntityState.Detached;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Update(Order order)
        {
            try
            {
                Task<Order> _order = GetOrderByOrderID(order.Id);
                if (_order != null)
                {
                    myDB.Entry<Order>(order).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                    myDB.Entry(order).State = EntityState.Detached;
                }
                else
                {
                    throw new Exception("The order not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Remove(Order order)
        {
            try
            {
                Task<Order> _order = GetOrderByOrderID(order.Id);
                if (_order != null)
                {
                    order.DeleteFlag = 1;
                    myDB.Entry<Order>(order).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The order not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
