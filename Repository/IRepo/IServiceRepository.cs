using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface IServiceRepository
    {
        List<Service> GetValidServices(Guid Id);
        IQueryable<Service> GetAllServicesByHostID(string Id);
        List<Service> GetAllServices();
        List<ServiceType> GetAllServiceTypes();
        Service GetServiceByServiceID(Guid Id);
        Task<Service> GetServiceByServiceIDAndHostID(Guid Id, string HostID);
        ServiceType GetServiceTypeByServiceTypeID(Guid Id);
        Task AddNew(Service service);
        Task Update(Service service);
        Task Remove(Service service);
    }
}
