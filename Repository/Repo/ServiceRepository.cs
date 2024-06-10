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
    public class ServiceRepository : IServiceRepository
    {
        public Task AddNew(Service service) => ServiceDAO.Instance.AddNew(service); 

        public List<Service> GetAllServices() => ServiceDAO.Instance.GetAllServices();

        public IQueryable<Service> GetAllServicesByHostID(string Id) => ServiceDAO.Instance.GetAllServicesByHostID(Id);

        public List<ServiceType> GetAllServiceTypes() => ServiceDAO.Instance.GetAllServiceTypes();

        public Service GetServiceByServiceID(Guid Id) => ServiceDAO.Instance.GetServiceByServiceID(Id);

        public Task<Service> GetServiceByServiceIDAndHostID(Guid Id, string HostID) => ServiceDAO.Instance.GetServiceByServiceIDAndHostID(Id, HostID);

        public ServiceType GetServiceTypeByServiceTypeID(Guid Id) => ServiceDAO.Instance.GetServiceTypeByServiceTypeID(Id);

        public List<Service> GetValidServices(Guid Id) => ServiceDAO.Instance.GetValidServices(Id);

        public Task Remove(Service service) => ServiceDAO.Instance.Remove(service); 

        public Task Update(Service service) => ServiceDAO.Instance.Update(service); 
    }
}
