using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class PlaceDAO
    {
        BirthdayPartyBookingContext myDB = new BirthdayPartyBookingContext();
        private static PlaceDAO instance = null;
        private static readonly object instanceLock = new object();
        public static PlaceDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PlaceDAO();
                    }
                    return instance;
                }
            }
        }
        public IQueryable<Place> GetAllPlace(string Id)
        {
            try
            {              
                var places = myDB.Places.AsNoTracking().Where(p => p.HostId.ToString() == Id);
                return places;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Place> GetAllPlaceByHostID(Guid Id)
        {
            try
            {
                var places =  myDB.Places.AsNoTracking().Where(p => p.HostId == Id && p.DeleteFlag == 0)
                                          .Include(p => p.Host)
                                          .ToList();
                return places;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Place> GetAllPlaceByHostIDAndPlaceID(string HostId, Guid placeId)
        {
            Place places = new Place();
            try
            {
                places = await myDB.Places.AsNoTracking().Where(p => p.HostId.ToString() == HostId)
                                                         .Include(p => p.Host).FirstOrDefaultAsync(m => m.Id == placeId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return places;
        }

        public async Task<Place> GetPlaceByPlaceID(Guid placeId)
        {
            Place places = new Place();
            try
            {
                places = await myDB.Places.AsNoTracking().Include(p => p.Host).FirstOrDefaultAsync(m => m.Id == placeId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return places;
        }

        public async Task AddNew(Place place)
        {
            try
            {
                myDB.Places.Add(place);
                await myDB.SaveChangesAsync();
                myDB.Entry(place).State = EntityState.Detached;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Update(Place place)
        {
            try
            {
                Task<Place> _place =GetPlaceByPlaceID(place.Id);
                if (_place != null)
                {
                    myDB.Entry<Place>(place).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                    myDB.Entry(place).State = EntityState.Detached;
                }
                else
                {
                    throw new Exception("The place not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Remove(Place place)
        {
            try
            {
                Task<Place> _place = GetPlaceByPlaceID(place.Id);
                if (_place != null)
                {
                    place.DeleteFlag = 1;
                    myDB.Entry<Place>(place).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The place not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
