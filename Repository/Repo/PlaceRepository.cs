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
    public class PlaceRepository : IPlaceRepository
    {
        public Task AddNew(Place place) => PlaceDAO.Instance.AddNew(place); 

        public IQueryable<Place> GetAllPlace(string Id) => PlaceDAO.Instance.GetAllPlace(Id);

        public List<Place> GetAllPlaceByHostID(Guid Id) => PlaceDAO.Instance.GetAllPlaceByHostID(Id);

        public Task<Place> GetAllPlaceByHostIDAndPlaceID(string HostId, Guid placeId) => PlaceDAO.Instance.GetAllPlaceByHostIDAndPlaceID(HostId, placeId);

        public Task<Place> GetPlaceByPlaceID(Guid placeId) => PlaceDAO.Instance.GetPlaceByPlaceID(placeId);

        public Task Remove(Place place) => PlaceDAO.Instance.Remove(place);

        public Task Update(Place place) => PlaceDAO.Instance.Update(place); 
    }
}
