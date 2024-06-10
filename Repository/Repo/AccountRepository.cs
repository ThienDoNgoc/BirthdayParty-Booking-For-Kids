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
    public class AccountRepository : IAccountRepository
    {
        public void AddNew(Account account) => AccountDAO.Instance.AddNew(account);

        public bool CheckEmailExist(string email) => AccountDAO.Instance.CheckEmailExist(email);

        public Account CheckLogin(string Email, string Password) => AccountDAO.Instance.CheckLogin(Email, Password);

        public Account GetAccountByAccountId(string id) => AccountDAO.Instance.GetAccountByAccountId(id);

        public Task<IList<Account>> GetAccounts(string searchString) => AccountDAO.Instance.GetAccounts(searchString);

        public Task<List<Account>> GetAllAccounts() => AccountDAO.Instance.GetAllAccounts();

        public Task<List<Account>> GetAllActiveAccounts() => AccountDAO.Instance.GetAllActiveAccounts();

        public Task<List<Account>> GetAllActiveHosts() => AccountDAO.Instance.GetAllActiveHosts();

        public Task Update(Account account) => AccountDAO.Instance.Update(account);
    }
}
