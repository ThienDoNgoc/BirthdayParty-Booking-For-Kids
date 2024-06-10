using BusinessObject;
using BusinessObject.ReportData;
using DAO;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class OrderRepository : IOrderRepository
    {
        public void AddNew(Order order) => OrderDAO.Instance.AddNew(order);

        public bool CheckOrderExist(Order order) => OrderDAO.Instance.CheckOrderExist(order);

        public Task<List<Order>> GetAllActiveorders() => OrderDAO.Instance.GetAllActiveorders();

        public Task<List<HostOrderSummary>> GetHostOrderSummaries() => OrderDAO.Instance.GetHostOrderSummaries();

        public Task<List<HostOrderSummary>> GetHostOrderSummariesByDate(DateTime startDate, DateTime endDate) => OrderDAO.Instance.GetHostOrderSummariesByDate(startDate, endDate);

        public IQueryable<Order> GetOrderByCustomerID(string id) => OrderDAO.Instance.GetOrderByCustomerID(id);

        public Task<List<Order>> GetOrderByDate(DateTime startDate, DateTime endDate) => OrderDAO.Instance.GetOrderByDate(startDate, endDate);

        public IQueryable<Order> GetOrderByHostID(string id) => OrderDAO.Instance.GetOrderByHostID(id);

        public Task<Order> GetOrderByOrderID(Guid id) => OrderDAO.Instance.GetOrderByOrderID(id);

        public Task<List<Order>> GetOrderForReport() => OrderDAO.Instance.GetOrderForReport();

        public Task<List<Order>> GetOrdersByStatus(int status) => OrderDAO.Instance.GetOrdersByStatus(status);

        public Task Remove(Order order) => OrderDAO.Instance.Remove(order);
        public Task Update(Order order) => OrderDAO.Instance.Update(order);
    }
}
