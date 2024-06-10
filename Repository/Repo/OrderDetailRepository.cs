using BusinessObject;
using DAO;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddNew(OrderDetail orderDetail) => OrderDetailDAO.Instance.AddNew(orderDetail);

        public Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid id) => OrderDetailDAO.Instance.GetOrderDetailByOrderID(id);
    }
}
