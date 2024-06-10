using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ServiceDAO
    {
        BirthdayPartyBookingContext myDB = new BirthdayPartyBookingContext();
        private static ServiceDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ServiceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceDAO();
                    }
                    return instance;
                }
            }
        }
        public List<Service> GetValidServices(Guid Id)
        {
            List<Service> services = new List<Service>();
            try
            {
                services = myDB.Services.AsNoTracking().Where(p => p.HostId == Id && p.DeleteFlag == 0).Include(s=>s.ServiceType).OrderBy(s => s.ServiceTypeId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return services;
        }

        public IQueryable<Service> GetAllServicesByHostID(string Id)
        {
            try
            {
                var services = myDB.Services.AsNoTracking().Where(s => s.HostId.ToString() == Id)
                                                           .Include(s => s.Host)
                                                           .Include(s => s.ServiceType);
                return services;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<Service> GetAllServices()
        {
            List<Service> services = new List<Service>();
            try
            {
                services = myDB.Services.AsNoTracking().Where(p => p.DeleteFlag == 0).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return services;
        }
        public List<ServiceType> GetAllServiceTypes()
        {
            List<ServiceType> serviceTypes = new List<ServiceType>();
            try
            {
                serviceTypes = myDB.ServiceTypes.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return serviceTypes;
        }
        public Service GetServiceByServiceID(Guid Id)
        {
            Service service = new Service();
            try
            {
                service = myDB.Services.AsNoTracking().FirstOrDefault(s => s.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return service;
        }

        public async Task<Service> GetServiceByServiceIDAndHostID(Guid Id, string HostID)
        {
            Service service = new Service();
            try
            {
                service = await myDB.Services.AsNoTracking().Where(s => s.HostId.ToString() == HostID && s.DeleteFlag == 0)
                .Include(s => s.Host)
                .Include(s => s.ServiceType).FirstOrDefaultAsync(m => m.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return service;
        }

        public ServiceType GetServiceTypeByServiceTypeID(Guid Id)
        {
            ServiceType serviceType = new ServiceType();
            try
            {
                serviceType = myDB.ServiceTypes.AsNoTracking().FirstOrDefault(s => s.Id == Id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return serviceType;
        }
        public async Task AddNew(Service service)
        {
            try
            {
                myDB.Services.Add(service);
                await myDB.SaveChangesAsync();
                myDB.Entry(service).State = EntityState.Detached;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Update(Service service)
        {
            try
            {
                Service _service = GetServiceByServiceID(service.Id);
                if (_service != null)
                {
                    myDB.Entry<Service>(service).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                    myDB.Entry(service).State = EntityState.Detached;
                }
                else
                {
                    throw new Exception("The service not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Remove(Service service)
        {
            try
            {
                Service _service = GetServiceByServiceID(service.Id);
                if (_service != null)
                {
                    service.DeleteFlag = 1;
                    myDB.Entry<Service>(service).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The service not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
