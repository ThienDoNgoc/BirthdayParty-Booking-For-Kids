using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetOrderDetailByOrderID(Guid id);
        void AddNew(OrderDetail orderDetail);
    }
}
