using BusinessObject;
using BusinessObject.ReportData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllActiveorders();
        Task<List<Order>> GetOrderForReport();
        Task<List<Order>> GetOrderByDate(DateTime startDate, DateTime endDate);
        Task<List<HostOrderSummary>> GetHostOrderSummaries();
        Task<List<Order>> GetOrdersByStatus(int status);
        Task<List<HostOrderSummary>> GetHostOrderSummariesByDate(DateTime startDate, DateTime endDate);
        IQueryable<Order> GetOrderByHostID(string id);
        IQueryable<Order> GetOrderByCustomerID(string id);

        Task<Order> GetOrderByOrderID(Guid id);
        bool CheckOrderExist(Order order);
        void AddNew(Order order);
        Task Update(Order order);
        Task Remove(Order order);
        
        

    }
}
