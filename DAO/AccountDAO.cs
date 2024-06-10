using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class AccountDAO
    {
        BirthdayPartyBookingContext myDB = new BirthdayPartyBookingContext();
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();
        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<List<Account>> GetAllActiveAccounts ()
        {
            List<Account> accounts;
            try
            {
                accounts = await myDB.Accounts.AsNoTracking().Where(s => s.DeleteFlag == 0).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accounts;
        }

        public async Task<IList<Account>> GetAccounts(string searchString)
        {
            return await myDB.Accounts.Where(a => a.Name.Contains(searchString))
                                      .ToListAsync();
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            List<Account> accounts;
            try
            {
                accounts = await myDB.Accounts.AsNoTracking().OrderBy(s => s.Role).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accounts;
        }

        public async Task<List<Account>> GetAllActiveHosts()
        {
            List<Account> accounts;
            try
            {
                accounts = await myDB.Accounts.AsNoTracking().Where(a => a.Role == 2 && a.DeleteFlag == 0).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accounts;
        }
        public Account GetAccountByAccountId(string id)
        {
            Account accounts;
            try
            {
                accounts =  myDB.Accounts.AsNoTracking().FirstOrDefault(c => c.Id.ToString() == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accounts;
        }
        public Account CheckLogin(string Email, string Password)
        {
            Account account = null;
            try
            {
                account = myDB.Accounts.AsNoTracking().Where(s => s.Email == Email && s.Password == Password && s.DeleteFlag == 0).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }
        public bool CheckEmailExist(string email)
        {
            bool check = false;
            try
            {
                var customers = myDB.Accounts.AsNoTracking().Where(s => s.DeleteFlag == 0 && s.Email == email).FirstOrDefault();
                if (customers!=null)
                {
                    check = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return check;
        }

        public void AddNew(Account account)
        {
            try
            {
                myDB.Accounts.Add(account);
                myDB.SaveChanges();
                myDB.Entry(account).State = EntityState.Detached;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(Account account)
        {
            try
            {
                Account _account = GetAccountByAccountId(account.Id.ToString());
                if (_account != null)
                {
                    myDB.Entry<Account>(account).State = EntityState.Modified;
                    await myDB.SaveChangesAsync();
                    myDB.Entry(account).State = EntityState.Detached;
                }
                else
                {
                    throw new Exception("The Account not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
